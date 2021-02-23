using System;
using System.IO;
using Iced.Intel;
using System.Runtime.InteropServices;
using static Iced.Intel.AssemblerRegisters;

namespace L2Robot
{
    public class MixedPackets
    {
        //private long _seed;
        public static uint[] tmp = new uint[16];
        private byte[] PacketIDs = new byte[256];//209
        private ushort[] SuperIDs = new ushort[512];//78
        //private int PacketIDLimit = 0xFF;
        //private int SuperIDLimit = 0xFFFF;//0x80;//0x4e

        private int magic;// = 0x18677494; //40 07 AF 64
        GameData gamedata;

        private IntPtr ThisCallCodes;
        private IntPtr ThisCallParams;
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(int hProcess, int lpAddress, int dwSize, AllocationType flAllocationType, MemoryProtection flProtect);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(int hProcess, int lpAddress,uint dwSize, uint dwFreeType);



        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate uint Delegate_ThisCall(IntPtr Params);
        private static Delegate_ThisCall delegate_ThisCall;

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate uint Delegate_ThisCallEx(IntPtr Params);
        private static Delegate_ThisCallEx delegate_ThisCallEx;
        public const uint MEM_RELEASE = 0x8000;



        public MixedPackets(GameData gamedata, int magic)
        {
            uint ret;
            long tmp_magic = magic;
            for (int i = 0; i < 16; i++)
            {
                //tmp[i] = (uint)i;
                tmp[i] = (uint)tmp_magic;
                tmp_magic = tmp_magic * 2 + 0x33;
            }

            ThisCall();
            ThisCallEx();

            SwitchCMD(0x11);
            SwitchCMD(0x12);
            SwitchCMD(0xB1);
            SwitchCMD(0xD0);
            SwitchExCMD(0x70);
            SwitchExCMD(0x71);

            Console.WriteLine("Magic=0x{0:X}", magic);
            Console.WriteLine("resultp[CMD]={0}", BitConverter.ToString(PacketIDs));
            for (int j = 0; j < 64; j++)
            {
                Console.WriteLine("result[EXCMD]=0x{0:X}", SuperIDs[j]);
                //Console.WriteLine("result[EXCMD]={0}", BitConverter.ToString(MixMixedPackets.resultEx));
            }
        }


        private int GetIndexOf(int Value, byte[] IdTable)
        {
            int i;
            for (i = 0; i < IdTable.Length; i++)
            {
                if (IdTable[i] == Value)
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetIndexOfSuper(int Value, ushort[] IdTable)
        {
            int i;
            for (i = 0; i < IdTable.Length; i++)
            {
                if (IdTable[i] == Value)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Encrypt0(ByteBuffer Packet)
        {
            ushort sdata, rdata;
            try
            {

                if (Packet.GetByte(0) == (byte)PClient.EXPacket)
                {
                    sdata = (ushort)((Packet.GetByte(0)) | (Packet.GetByte(1) << 8));
                    rdata = (ushort)GetIndexOfSuper(sdata, SuperIDs);
                    Packet.SetByte(1, (byte)(rdata & 0xFF));
                    Packet.SetByte(2, (byte)((rdata & 0xFF00) >> 8));
                }
                else
                {
                    Packet.SetByte(0, (byte)GetIndexOf(Packet.GetByte(0), PacketIDs));
                }
            }
            catch
            {

            }
        }

        public void Decrypt0(byte[] Packet)
        {
            ushort sdata, rdata;
            try
            {

                if (Packet[0] == (byte)PClient.EXPacket)
                {
                    sdata = (ushort)((Packet[1]) | (Packet[2] << 8));
                    rdata = SuperIDs[sdata];
                    Packet[1] = (byte)(rdata & 0xFF);
                    Packet[2] = (byte)((rdata & 0xFF00) >> 8);
                }
                else
                {
                    
                    Packet[0] = PacketIDs[Packet[0]];
                }
            }
            catch
            {
            }
        }


        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        public void SwitchCMD(byte cmd)
        {
            int i;
            for (i = 0; i < PacketIDs.Length; i++)
            {
                if(PacketIDs[i] == cmd)
                {
                    PacketIDs[i] = PacketIDs[cmd];
                    PacketIDs[cmd] = cmd;
                    Console.WriteLine("SwitchCMD - 0x{0:x}:{1}", PacketIDs[i], i);
                    break;
                }
            }
        }

        public void SwitchExCMD(ushort cmd)
        {
            int i;
            for (i = 0; i < SuperIDs.Length; i++)
            {
                if (SuperIDs[i] == cmd)
                {
                    SuperIDs[i] = SuperIDs[cmd];
                    SuperIDs[cmd] = cmd;
                    Console.WriteLine("SwitchExCMD - 0x{0:x}:{1}", SuperIDs[i], i);
                    break;
                }
            }
        }

        public void InitThisCall()
        {
           
            ThisCallCodes = VirtualAllocEx(-1, 0, 400, AllocationType.Commit | AllocationType.TopDown, MemoryProtection.ExecuteReadWrite);
            using (var stream = new MemoryStream())
            {
                var asm = new Assembler(32);
                var label_1 = asm.CreateLabel();

                asm.push(ebp);
                asm.mov(ebp, esp); //ebp = stack
                asm.sub(esp, 0x14);
                asm.mov(eax, __dword_ptr[ebp + 8]); //Get Parameter Pointer
                
                asm.mov(ecx, __dword_ptr[eax + 1 * 4]); //Get Tmp Addr
                asm.mov(eax, __dword_ptr[eax + 0 * 4]); //Get Result Addr

                asm.mov(__dword_ptr[ebp - 0x14], eax);  //Get Result Addr //ebp+8

                //asm.mov(eax, __dword_ptr[ebp + 8]);
                asm.mov(__dword_ptr[ebp - 0xC], 0x0);

                //asm.mov(eax, __dword_ptr[ecx + 15*4]); //Test
               
                asm.push(ebx);
                asm.dec(eax);
                asm.mov(__dword_ptr[ebp - 4], 2);
                asm.push(esi);

                asm.mov(__dword_ptr[ebp - 8], eax);
                asm.push(edi);
                
                asm.Label(ref label_1);
                asm.mov(esi, __dword_ptr[ebp - 0xC]);
                asm.mov(edx, __dword_ptr[ecx + esi * 4]);
                asm.lea(eax, __dword_ptr[esi - 3]);
                
                asm.and(eax, 0xF);
                asm.lea(ebx, __dword_ptr[edx + edx]);
                //asm.mov(eax, ebx); Test
                
                //asm.mov(eax, ebx);
                
                asm.mov(eax, __dword_ptr[ecx + eax * 4]);

                asm.xor(ebx, eax);
                asm.shl(ebx, 0xF);
                asm.xor(ebx, eax);
                asm.lea(eax, __dword_ptr[esi - 7]);

                

                asm.and(eax, 0xF);
                asm.xor(ebx, edx);
                asm.mov(edi, __dword_ptr[ecx + eax * 4]);
                asm.mov(eax, edi);

                
                asm.shr(eax, 0xB);
                asm.xor(edi, eax);
                asm.mov(eax, ebx);
                asm.xor(eax, edi);

                asm.shl(edi, 0xA);
                asm.mov(__dword_ptr[ecx + esi * 4], eax);
                
                asm.xor(edi, ebx);
                asm.mov(esi, eax);

                asm.shl(edi, 0x10);
                asm.and(esi, 0xFED22169);
                asm.shl(esi, 5);
                asm.xor(esi, eax);
                //asm.mov(eax, esi); //TEST

                
                asm.mov(eax, __dword_ptr[ebp - 0xC]);
                asm.dec(eax);
                asm.and(eax, 0xF);
                asm.mov(__dword_ptr[ebp - 0xC], eax);

                asm.lea(edx, __dword_ptr[ecx + eax * 4]);
                asm.xor(edi, __dword_ptr[edx]);
                asm.shl(edi, 2);
                asm.xor(edi, esi);
                //asm.mov(eax, edi); //TEST
                

                asm.mov(esi, __dword_ptr[ebp - 4]);
                asm.xor(edi, ebx);
                asm.xor(__dword_ptr[edx], edi);
                asm.xor(edx, edx);
                  
                asm.mov(eax, __dword_ptr[ebp - 0xC]);
                asm.mov(edi, __dword_ptr[ebp - 8]);
                asm.mov(eax, __dword_ptr[ecx + eax * 4]);
                asm.div(esi);

                //asm.mov(eax, __dword_ptr[edi]); //TEST
                //asm.mov(eax, esi); //TEST

                
                asm.and(ebx, 0xFFFFFF00);
                //asm.mov(eax, ebx); //TEST
                //asm.mov(eax, __dword_ptr[edi + esi]);
                asm.mov(__dword_ptr[ebp - 0x10], ebx);
                asm.mov(ebx, __dword_ptr[edi + esi]);
                asm.and(ebx, 0x000000FF);
                asm.or(ebx, __dword_ptr[ebp - 0x10]);

                //asm.mov(bl, __dword_ptr[edi + esi]); //Dirty work
                
                asm.add(edx, __dword_ptr[ebp - 0x14]);
                //asm.mov(eax, edx); //TEST
                
                //asm.mov(al, __dword_ptr[edx]); //Dirty work
                
                asm.and(eax, 0xFFFFFF00);
                asm.mov(__dword_ptr[ebp - 0x10], eax);
                asm.mov(eax, __dword_ptr[edx]);
                asm.and(eax, 0x000000FF);
                asm.or(eax, __dword_ptr[ebp - 0x10]);
                asm.mov(__dword_ptr[edi + esi], al);
                asm.inc(esi);
                asm.mov(__dword_ptr[edx], bl);
                asm.mov(__dword_ptr[ebp - 4], esi);
                asm.cmp(esi, 0xD1);
                asm.jbe(label_1);

                asm.pop(edi);
                asm.pop(esi);
                asm.pop(ebx);                
                asm.mov(esp, ebp);
                asm.pop(ebp);
                asm.ret();
              
                asm.Assemble(new StreamCodeWriter(stream), (ulong)ThisCallCodes.ToInt64());
                var code = stream.ToArray();
                Marshal.Copy(code, 0, ThisCallCodes, code.Length);
            }
            delegate_ThisCall = Marshal.GetDelegateForFunctionPointer<Delegate_ThisCall>(ThisCallCodes);
            ThisCallParams = Marshal.AllocHGlobal(4 * 20);
            //Console.WriteLine($"ThisCall:{ThisCallCodes.ToString("X8")}:{ThisCallParams.ToString("X8")}");
        }

        public uint ThisCall()
        {
            //IntPtr p1 = IntPtr.Zero;
            uint p1 = 0;
            IntPtr addr_tmp;
            IntPtr addr_result;

            for (int i = 0; i < 256; i++)
            {
                PacketIDs[i] = (byte)i;
            }

            GCHandle hObject_tmp = GCHandle.Alloc(tmp, GCHandleType.Pinned);
            addr_tmp = hObject_tmp.AddrOfPinnedObject();

            GCHandle hObject_result = GCHandle.Alloc(PacketIDs, GCHandleType.Pinned);
            addr_result = hObject_result.AddrOfPinnedObject();

            //GCHandle hObject_result = GCHandle.Alloc(result, GCHandleType.Pinned);
            //addr_result = hObject_result.AddrOfPinnedObject();


            if (delegate_ThisCall == null) InitThisCall();
            lock (delegate_ThisCall)
            {
                try
                {
                    Marshal.WriteInt32(ThisCallParams + 0 * 4, (int)addr_result);
                    Marshal.WriteInt32(ThisCallParams + 1 * 4, (int)addr_tmp);
                    p1 = delegate_ThisCall.Invoke(ThisCallParams);
                }
                catch (Exception err)
                {
                }
                finally
                {
                    VirtualFreeEx(-1, (int)ThisCallCodes, 0, MEM_RELEASE);
                    Marshal.FreeHGlobal(ThisCallParams);
                    if (hObject_tmp.IsAllocated)
                        hObject_tmp.Free();
                    if (hObject_result.IsAllocated)
                        hObject_result.Free();
                }
            }
            return p1;
        }


        public void InitThisCallEx()
        {

            ThisCallCodes = VirtualAllocEx(-1, 0, 400, AllocationType.Commit | AllocationType.TopDown, MemoryProtection.ExecuteReadWrite);
            using (var stream = new MemoryStream())
            {
                var asm = new Assembler(32);
                var label_1 = asm.CreateLabel();

                asm.push(ebp);
                asm.mov(ebp, esp); //ebp = stack
                asm.sub(esp, 0x14);
                asm.mov(eax, __dword_ptr[ebp + 8]); //Get Parameter Pointer

                asm.mov(ecx, __dword_ptr[eax + 1 * 4]); //Get Tmp Addr
                asm.mov(eax, __dword_ptr[eax + 0 * 4]); //Get ResultEx Addr
                asm.mov(__dword_ptr[ebp - 0x14], eax);  //Get ResultEx Addr //ebp+8 | ecx tmpAddr
                //asm.mov(eax, __dword_ptr[eax+4]); //Test
                
                
                //asm.mov(eax, __dword_ptr[ebp + 8]);
                asm.mov(__dword_ptr[ebp - 0xC], 0x0);  //[ecx + 40]

                //asm.mov(eax, __dword_ptr[ecx + 15*4]); //Test

                asm.push(ebx);
                asm.add(eax, 2);
                asm.mov(__dword_ptr[ebp - 4], 2);
                asm.push(esi);

                asm.mov(__dword_ptr[ebp - 8], eax);
                asm.push(edi);
                           
                asm.lea(esp, __dword_ptr[esp + 00000000]);

                asm.Label(ref label_1);
                asm.mov(esi, __dword_ptr[ebp - 0xC]);
                asm.mov(edx, __dword_ptr[ecx + esi * 4]);
                asm.lea(eax, __dword_ptr[esi - 3]);
                
                asm.and(eax, 0xF);
                asm.lea(ebx, __dword_ptr[edx + edx]);
                //asm.mov(eax, ebx); Test

                //asm.mov(eax, ebx);

                asm.mov(eax, __dword_ptr[ecx + eax * 4]);

                asm.xor(ebx, eax);
                asm.shl(ebx, 0xF);
                asm.xor(ebx, eax);
                //asm.mov(eax, ebx); //Test

                
                asm.lea(eax, __dword_ptr[esi - 7]);
                asm.and(eax, 0xF);
                asm.xor(ebx, edx);
                asm.mov(edi, __dword_ptr[ecx + eax * 4]);
                asm.mov(eax, edi);


                asm.shr(eax, 0xB);
                asm.xor(edi, eax);
                asm.mov(eax, ebx);
                asm.xor(eax, edi);

                
                asm.shl(edi, 0xA);
                asm.mov(__dword_ptr[ecx + esi * 4], eax);

                asm.xor(edi, ebx);
                asm.mov(esi, eax);

                asm.shl(edi, 0x10);
                asm.and(esi, 0xFED22169);
                asm.shl(esi, 5);
                asm.xor(esi, eax);
                //asm.mov(eax, esi); //TEST

                
                asm.mov(eax, __dword_ptr[ebp - 0xC]);
                asm.dec(eax);
                asm.and(eax, 0xF);
                asm.mov(__dword_ptr[ebp - 0xC], eax);

                asm.lea(edx, __dword_ptr[ecx + eax * 4]);
                asm.xor(edi, __dword_ptr[edx]);
                asm.shl(edi, 2);
                asm.xor(edi, esi);
                //asm.mov(eax, edi); //TEST
                
                asm.xor(edi, ebx);
                asm.mov(ebx, __dword_ptr[ebp - 4]);
                asm.xor(__dword_ptr[edx], edi);
                asm.xor(edx, edx);

                asm.mov(eax, __dword_ptr[ebp - 0xC]);
                asm.mov(edi, __dword_ptr[ebp - 8]);
                asm.mov(eax, __dword_ptr[ecx + eax * 4]);
                asm.div(ebx);
                
                //asm.mov(eax, __dword_ptr[edi]); //TEST
                //asm.mov(eax, esi); //TEST
                asm.mov(eax, __dword_ptr[ebp - 0x14]);
                asm.inc(ebx);
                asm.lea(esi, __dword_ptr[eax + edx * 2]);
                asm.mov(__dword_ptr[ebp - 4], ebx);
                //asm.mov(eax, edi); //TEST
                

                asm.movzx(edx, __word_ptr[edi]);
                //asm.mov(eax, edx); //TEST

                asm.and(eax, 0xFFFF0000);
                asm.mov(__dword_ptr[ebp - 0x10], eax);
                asm.mov(eax, __dword_ptr[esi]);
                asm.and(eax, 0x0000FFFF);
                asm.or(eax, __dword_ptr[ebp - 0x10]);
                
                //asm.mov(eax, __dword_ptr[edi]);

                //asm.mov(eax, __dword_ptr[edi]); //TEST

                
                asm.mov(__dword_ptr[edi], ax);

                //asm.mov(eax, __dword_ptr[edi]); //TEST
                
                
                asm.add(edi, 2);
                asm.mov(__dword_ptr[esi], dx);
                //asm.mov(eax, edx); //Test
                
                asm.mov(__dword_ptr[ebp - 8], edi);
                asm.cmp(ebx, 0x1F9);
                asm.jbe(label_1);
                
                
                 /*
                 asm.and(ebx, 0xFFFFFF00);
                 //asm.mov(eax, ebx); //TEST
                 //asm.mov(eax, __dword_ptr[edi + esi]);
                 asm.mov(__dword_ptr[ebp - 0x10], ebx);
                 asm.mov(ebx, __dword_ptr[edi + esi]);
                 asm.and(ebx, 0x000000FF);
                 asm.or(ebx, __dword_ptr[ebp - 0x10]);

                 //asm.mov(bl, __dword_ptr[edi + esi]); //Dirty work

                 asm.add(edx, __dword_ptr[ebp - 0x14]);
                 //asm.mov(eax, edx); //TEST

                 //asm.mov(al, __dword_ptr[edx]); //Dirty work

                 asm.and(eax, 0xFFFFFF00);
                 asm.mov(__dword_ptr[ebp - 0x10], eax);
                 asm.mov(eax, __dword_ptr[edx]);
                 asm.and(eax, 0x000000FF);
                 asm.or(eax, __dword_ptr[ebp - 0x10]);
                 asm.mov(__dword_ptr[edi + esi], al);
                 asm.inc(esi);
                 asm.mov(__dword_ptr[edx], bl);
                 asm.mov(__dword_ptr[ebp - 4], esi);
                 asm.cmp(esi, 0xD1);
                 asm.jbe(label_1);
                 */
                asm.pop(edi);
                asm.pop(esi);
                asm.pop(ebx);
                asm.mov(esp, ebp);
                asm.pop(ebp);
                asm.ret();

                asm.Assemble(new StreamCodeWriter(stream), (ulong)ThisCallCodes.ToInt64());
                var code = stream.ToArray();
                Marshal.Copy(code, 0, ThisCallCodes, code.Length);
            }
            delegate_ThisCallEx = Marshal.GetDelegateForFunctionPointer<Delegate_ThisCallEx>(ThisCallCodes);
            ThisCallParams = Marshal.AllocHGlobal(4 * 20);
            //Console.WriteLine($"ThisCall:{ThisCallCodes.ToString("X8")}:{ThisCallParams.ToString("X8")}");
        }

        public uint ThisCallEx()
        {
            //IntPtr p1 = IntPtr.Zero;
            uint p1 = 0;
            IntPtr addr_tmp;
            IntPtr addr_result;


            for (int i = 0; i < 512; i++)
            {
                //resultEx[2*i] = (byte)(i&0xFF);
                //resultEx[2*i + 1] = (byte)((i & 0xFF00) >> 8);
                SuperIDs[i] = (ushort)i;
            }

            GCHandle hObject_tmp = GCHandle.Alloc(tmp, GCHandleType.Pinned);
            addr_tmp = hObject_tmp.AddrOfPinnedObject();

            GCHandle hObject_result = GCHandle.Alloc(SuperIDs, GCHandleType.Pinned);
            addr_result = hObject_result.AddrOfPinnedObject();

            //GCHandle hObject_result = GCHandle.Alloc(result, GCHandleType.Pinned);
            //addr_result = hObject_result.AddrOfPinnedObject();


            if (delegate_ThisCallEx == null) InitThisCallEx();
            lock (delegate_ThisCallEx)
            {
                try
                {
                    Marshal.WriteInt32(ThisCallParams + 0 * 4, (int)addr_result);
                    Marshal.WriteInt32(ThisCallParams + 1 * 4, (int)addr_tmp);
                    p1 = delegate_ThisCallEx.Invoke(ThisCallParams);
                }
                catch (Exception err)
                {
                }
                finally
                {
                    VirtualFreeEx(-1, (int)ThisCallCodes, 0, MEM_RELEASE);
                    Marshal.FreeHGlobal(ThisCallParams);
                    if (hObject_tmp.IsAllocated)
                        hObject_tmp.Free();
                    if (hObject_result.IsAllocated)
                        hObject_result.Free();
                }
            }
            return p1;
        }

        //public ulong ToUInt64(this IntPtr ptr) => (ulong)ptr.ToInt64();
    }
}
