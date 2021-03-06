using System.Collections;
using System.Diagnostics;
using System.IO;

namespace L2Robot
{
    public partial class ScriptEngine
    {
        private string ReadToken(ref string inp)
        {
            string backup = inp;

            string token = Get_String(ref inp);

            if (isOp(token))
            {
                //we've got a token here
            }
            else
            {
                //we have a variable...
                //need to read over until we hit a token or the end of the line

                bool done = false;

                while (!done)
                {
                    string pre = inp;
                    string tmp = Get_String(ref inp);

                    if (isOp(tmp))
                    {
                        //we found a token...
                        //need to restore our string and get out of here

                        inp = pre;
                        done = true;
                    }
                    else
                    {
                        //if we have a null token... don't bother appending it
                        if (tmp.Length > 0)
                        {
                            //need to get the same value... but as a token, not an evaluated string
                            inp = pre;
                            tmp = Get_StringToken(ref inp);
                            token = token + " " + tmp;
                        }
                    }

                    if (inp.Length == 0)
                    {
                        //hit the end of the line...
                        done = true;
                    }
                }
            }

            return token;
        }

        private bool Evaluate(string inp)
        {
            ScriptVariable outv = new ScriptVariable();
            outv.Type = Var_Types.ASSIGNABLE;

            Assignment(outv, inp);

            switch (outv.Type)
            {
                case Var_Types.ASSIGNABLE:
                    if (System.Convert.ToInt64(outv.Value, System.Globalization.CultureInfo.InvariantCulture) >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Var_Types.INT:
                    if (System.Convert.ToInt64(outv.Value, System.Globalization.CultureInfo.InvariantCulture) >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case Var_Types.DOUBLE:
                    if (System.Convert.ToDouble(outv.Value, System.Globalization.CultureInfo.InvariantCulture) >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    Script_Error("CANNOT PERFORM BOOLEAN EVALUATION ON TYPE OF " + outv.Type);
                    break;
            }

            return false;
        }

        private void Assignment(ScriptVariable dest, string equation)
        {
            Queue parsed = new Queue();

            while (equation.Length > 0)
            {
                string token = ReadToken(ref equation);
                if (token.Length > 0)
                {
                    parsed.Enqueue(token);
                }
            }

            //now we have all our tokens in an array list...
            //time to evaluate
            ScriptVariable outv = ProcessData(parsed);

            switch (dest.Type)
            {
                case Var_Types.INT:
                    try
                    {
                        dest.Value = System.Convert.ToInt64(outv.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        dest.Value = 0;
                    }
                    break;
                case Var_Types.DOUBLE:
                    try
                    {
                        dest.Value = System.Convert.ToDouble(outv.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        dest.Value = 0D;
                    }
                    break;
                case Var_Types.STRING:
                    try
                    {
                        dest.Value = System.Convert.ToString(outv.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        dest.Value = "error in assingment casting from type of: " + outv.Type.ToString();
                    }
                    break;
                default:
                    dest.Value = outv.Value;
                    dest.Type = outv.Type;
                    break;
            }
        }

        private ScriptVariable ProcessData(Queue equation)
        {
            ArrayList tmpvars = new ArrayList();

            ScriptVariable outv = new ScriptVariable();
            outv.Type = Var_Types.ASSIGNABLE;

            ScriptVariable outi;

            Queue neweq = new Queue();

            //lets process all the paran bullshit first
            while (equation.Count > 0)
            {
                string token1 = equation.Dequeue().ToString();

                if (token1 == "(")
                {
                    int pcnt = 1;

                    Queue subeq = new Queue();

                    while (pcnt != 0)
                    {
                        string ntoken = equation.Dequeue().ToString();

                        if (ntoken == "(")
                        {
                            pcnt++;
                        }
                        if (ntoken == ")")
                        {
                            pcnt--;
                        }
                        if (pcnt != 0)
                        {
                            subeq.Enqueue(ntoken);
                        }
                    }

                    outi = new ScriptVariable();
                    outi.Type = Var_Types.ASSIGNABLE;
                    outi = ProcessData(subeq);

                    outi.Name = Globals.SCRIPT_OUT_VAR + Globals.Rando.Next(int.MaxValue);
                    while (VariableExists(outi.Name))
                    {
                        outi.Name = Globals.SCRIPT_OUT_VAR + Globals.Rando.Next(int.MaxValue);
                    }
                    tmpvars.Add(outi.Name);
                    Add_Variable(outi, StackHeight);

                    neweq.Enqueue(outi.Name);
                }
                else
                {
                    neweq.Enqueue(token1);
                }
            }

            //now we have a queue of pure tokens with no parans
            while (neweq.Count > 0)
            {
                string token1 = neweq.Dequeue().ToString();

                if (neweq.Count == 0)
                {
                    //we only had 1 parameter
                    outv = Get_Var(token1);
                }
                else
                {
                    outi = new ScriptVariable();
                    outi.Type = Var_Types.ASSIGNABLE;

                    string token2 = neweq.Dequeue().ToString();

                    if (isUnaryOp(token1.ToUpperInvariant()))
                    {
                        EvaluateUnary(outi, token1.ToUpperInvariant(), token2);
                    }
                    else if (isBinaryOp(token2.ToUpperInvariant()))
                    {
                        string token3 = neweq.Dequeue().ToString();

                        EvaluateBinary(outi, token2.ToUpperInvariant(), token1, token3);
                    }

                    //add our created value to the stack
                    outi.Name = Globals.SCRIPT_OUT_VAR + Globals.Rando.Next(int.MaxValue);
                    while (VariableExists(outi.Name))
                    {
                        outi.Name = Globals.SCRIPT_OUT_VAR + Globals.Rando.Next(int.MaxValue);
                    }
                    tmpvars.Add(outi.Name);
                    Add_Variable(outi, StackHeight);

                    //now we need to push this variable to the front of our queue via a temporary queue
                    Queue tmpeq = new Queue();
                    tmpeq.Enqueue(outi.Name);
                    while (neweq.Count > 0)
                    {
                        tmpeq.Enqueue(neweq.Dequeue());
                    }
                    neweq = tmpeq;
                }

            }

            //delete all our temporary variables
            foreach (string name in tmpvars)
            {
                Script_DELETE(name);
            }

            return outv;
        }

        private void EvaluateUnary(ScriptVariable dest_ob, string op, string var1)
        {
            long dest_i = 0;
            double dest_d = 0;
            uint id = 0;
            Var_Types cast = Var_Types.NULL;

            switch (op)
            {
                case "VARIABLE_DEFINED":
                case "IS_DEFINED":
                case "[]":
                case "??":
                    if (VariableExists(var1.ToUpperInvariant()))
                    {
                        dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                        cast = Var_Types.INT;
                    }
                    else
                    {
                        dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                        cast = Var_Types.INT;
                    }
                    break;
                case "IS_LOCKED":
                    if (Locks.ContainsKey(var1.ToUpperInvariant()))
                    {
                        dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                        cast = Var_Types.INT;
                    }
                    else
                    {
                        dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                        cast = Var_Types.INT;
                    }
                    break;
                default:
                    ScriptVariable source1_ob = Get_Var(var1);

                    switch (op)
                    {
                        case "":
                            dest_ob = source1_ob;
                            return;
                        case "SQRT":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = (long)System.Math.Sqrt(System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Sqrt(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "ABS":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = System.Math.Abs(System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Abs(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "SIN":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = (long)System.Math.Sin(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Sin(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "SINH":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = (long)System.Math.Sinh(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Sinh(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "ASIN":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = (long)System.Math.Asin(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Asin(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "COS":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = (long)System.Math.Cos(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Cos(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "COSH":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = (long)System.Math.Cosh(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Cosh(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "ACOS":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = (long)System.Math.Acos(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Acos(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "TAN":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = (long)System.Math.Tan(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Tan(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "TANH":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = (long)System.Math.Tanh(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Tanh(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "ATAN":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = (long)System.Math.Atan(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.INT;
                            }
                            else if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_d = System.Math.Atan(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                                cast = Var_Types.DOUBLE;
                            }
                            break;
                        case "~":
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = ~System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "IS_RUNNING":
                            bool found = false;
                            if (source1_ob.Type == Var_Types.STRING)
                            {
                                Process[] processlist = Process.GetProcesses();

                                foreach (Process theprocess in processlist)
                                {
                                    if (theprocess.ProcessName == source1_ob.Value.ToString())
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                            }

                            if (found)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            return;
                        case "IS_NOT_RUNNING":
                            found = false;
                            if (source1_ob.Type == Var_Types.STRING)
                            {
                                Process[] processlist = Process.GetProcesses();

                                foreach (Process theprocess in processlist)
                                {
                                    if (theprocess.ProcessName == source1_ob.Value.ToString())
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                            }

                            if (found)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "FILE_EXISTS":
                            found = false;
                            if (source1_ob.Type == Var_Types.STRING)
                            {
                                if (File.Exists(source1_ob.Value.ToString()))
                                {
                                    found = true;
                                }
                            }

                            if (found)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?I":
                            //is the variable an integer
                            if (source1_ob.Type == Var_Types.INT)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?D":
                            //is the variable a double
                            if (source1_ob.Type == Var_Types.DOUBLE)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?$":
                            //is the variable a string
                            if (source1_ob.Type == Var_Types.STRING)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?S":
                            //is the variable a sortedlist
                            if (source1_ob.Type == Var_Types.SORTEDLIST)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?A":
                            //is the variable an arraylist
                            if (source1_ob.Type == Var_Types.ARRAYLIST)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?ST":
                            //is the variable a stack
                            if (source1_ob.Type == Var_Types.QUEUE)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?Q":
                            //is the variable a queue
                            if (source1_ob.Type == Var_Types.QUEUE)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?FR":
                            //is the variable a FILEREADER
                            if (source1_ob.Type == Var_Types.FILEREADER)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?FW":
                            //is the variable a FILEWRITER
                            if (source1_ob.Type == Var_Types.FILEWRITER)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?B":
                            //is the variable a BYTEBUFFER
                            if (source1_ob.Type == Var_Types.BYTEBUFFER)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?W":
                            //is the variable a window
                            if (source1_ob.Type == Var_Types.WINDOW)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        case "?C":
                            //is the variable a user defined class
                            if (source1_ob.Type == Var_Types.CLASS)
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("TRUE").Value);
                                cast = Var_Types.INT;
                            }
                            else
                            {
                                dest_i = System.Convert.ToInt64(Get_Value("FALSE").Value);
                                cast = Var_Types.INT;
                            }
                            break;
                        default:
                            Script_Error("UNKNOWN MATH TYPE : " + op);
                            break;
                    }
                    break;
            }

            if (cast == Var_Types.NULL)
            {
                //we tried an operation that didn't succeed
                Script_Error("can't perform [" + op + "] on [" + var1 + "]");
            }

            switch (dest_ob.Type)
            {
                case Var_Types.INT:
                    switch (cast)
                    {
                        case Var_Types.INT:
                            dest_ob.Value = dest_i;
                            break;
                        case Var_Types.DOUBLE:
                            try
                            {
                                dest_ob.Value = System.Convert.ToInt64(dest_d);
                            }
                            catch
                            {
                                //can't cast NaN to int
                                dest_ob.Value = long.MinValue;
                            }
                            break;
                    }
                    break;
                case Var_Types.DOUBLE:
                    switch (cast)
                    {
                        case Var_Types.INT:
                            dest_ob.Value = System.Convert.ToDouble(dest_i);
                            break;
                        case Var_Types.DOUBLE:
                            dest_ob.Value = dest_d;
                            break;
                    }
                    break;
                case Var_Types.STRING:
                    switch (cast)
                    {
                        case Var_Types.INT:
                            dest_ob.Value = System.Convert.ToString(dest_i);
                            break;
                        case Var_Types.DOUBLE:
                            dest_ob.Value = System.Convert.ToString(dest_d);
                            break;
                    }
                    break;
                case Var_Types.ASSIGNABLE:
                    switch (cast)
                    {
                        case Var_Types.INT:
                            dest_ob.Value = dest_i;
                            dest_ob.Type = Var_Types.INT;
                            break;
                        case Var_Types.DOUBLE:
                            dest_ob.Value = dest_d;
                            dest_ob.Type = Var_Types.DOUBLE;
                            break;
                    }
                    break;
                default:
                    //script error
                    Script_Error("couldn't cast type " + cast.ToString() + " to type " + dest_ob.Type.ToString());
                    break;
            }
        }

        private void EvaluateBinary(ScriptVariable dest_ob, string op, string var1, string var2)
        {
            ScriptVariable source1_ob = Get_Var(var1);
            ScriptVariable source2_ob = Get_Var(var2);

            long dest_i = 0;
            double dest_d = 0;
            string dest_str = "";

            Var_Types cast = Var_Types.NULL;

            switch (op)
            {
                case "+":
                    if (source1_ob.Type == Var_Types.STRING)
                    {
                        dest_str = System.Convert.ToString(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) + System.Convert.ToString(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.STRING;
                    }
                    else if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) + System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_d = System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) + System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.DOUBLE;
                    }
                    break;
                case "-":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) - System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_d = System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) - System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.DOUBLE;
                    }
                    break;
                case "*":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) * System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_d = System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) * System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.DOUBLE;
                    }
                    break;
                case "/":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) / System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_d = System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) / System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.DOUBLE;
                    }
                    break;
                case "^":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = (long)System.Math.Pow(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture), System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_d = System.Math.Pow(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture), System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                        cast = Var_Types.DOUBLE;
                    }
                    break;
                case "%":
                    long tmp, tmp2;
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        tmp = System.Math.DivRem(System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture), System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture), out tmp2);
                        dest_i = tmp2;
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        tmp = System.Math.DivRem(System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture), System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture), out tmp2);
                        dest_d = tmp2;
                        cast = Var_Types.DOUBLE;
                    }
                    break;
                case "LOG":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = (long)System.Math.Log(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture), System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_d = System.Math.Log(System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture), System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture));
                        cast = Var_Types.DOUBLE;
                    }
                    break;
                case "||":
                case "OR":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        if (System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) != 0 || System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) != 0)
                        {
                            dest_i = 1;
                        }
                        else
                        {
                            dest_i = 0;
                        }

                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        if (System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) != 0 || System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) != 0)
                        {
                            dest_i = 1;
                        }
                        else
                        {
                            dest_i = 0;
                        }

                        cast = Var_Types.INT;
                    }
                    break;
                case "&&":
                case "AND":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        if (System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) != 0 && System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) != 0)
                        {
                            dest_i = 1;
                        }
                        else
                        {
                            dest_i = 0;
                        }

                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        if (System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) != 0 && System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) != 0)
                        {
                            dest_i = 1;
                        }
                        else
                        {
                            dest_i = 0;
                        }

                        cast = Var_Types.INT;
                    }
                    break;
                case "|":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) | System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.INT;
                    }
                    break;
                case "&":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) & System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.INT;
                    }
                    break;
                case "<<":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt32(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) << System.Convert.ToInt32(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.INT;
                    }
                    break;
                case ">>":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt32(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) >> System.Convert.ToInt32(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.INT;
                    }
                    break;
                case "XOR":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ^ System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture);
                        cast = Var_Types.INT;
                    }
                    break;
                case "==":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) == System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_i = System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) == System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.STRING)
                    {
                        dest_i = System.String.Compare(source1_ob.Value.ToString(), source2_ob.Value.ToString()) == 0 ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    else
                    {
                        if (source1_ob.Value == source2_ob.Value)
                        {
                            dest_i = 1;
                            cast = Var_Types.INT;
                        }
                        else
                        {
                            dest_i = 1;
                            cast = Var_Types.INT;
                        }
                    }
                    break;
                case "!=":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) != System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_i = System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) != System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.STRING)
                    {
                        dest_i = System.String.Compare(source1_ob.Value.ToString(), source2_ob.Value.ToString()) != 0 ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    else
                    {
                        if (source1_ob.Value != source2_ob.Value)
                        {
                            dest_i = 1;
                            cast = Var_Types.INT;
                        }
                        else
                        {
                            dest_i = 1;
                            cast = Var_Types.INT;
                        }
                    }
                    break;
                case "<":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) < System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_i = System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) < System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    break;
                case "<=":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) <= System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_i = System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) <= System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    break;
                case ">":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) > System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_i = System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) > System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    break;
                case ">=":
                    if (source1_ob.Type == Var_Types.INT)
                    {
                        dest_i = System.Convert.ToInt64(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) >= System.Convert.ToInt64(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    else if (source1_ob.Type == Var_Types.DOUBLE)
                    {
                        dest_i = System.Convert.ToDouble(source1_ob.Value, System.Globalization.CultureInfo.InvariantCulture) >= System.Convert.ToDouble(source2_ob.Value, System.Globalization.CultureInfo.InvariantCulture) ? 1 : 0;
                        cast = Var_Types.INT;
                    }
                    break;
                default:
                    Script_Error("UNKNOWN MATH TYPE : " + op);
                    break;
            }

            if (cast == Var_Types.NULL)
            {
                //we tried an operation that didn't succeed
                Script_Error("invalid type: can't perform [" + source1_ob.Name + "] [" + op + "] [" + source2_ob.Name + "]");
            }


            switch (dest_ob.Type)
            {
                case Var_Types.INT:
                    switch (cast)
                    {
                        case Var_Types.INT:
                            dest_ob.Value = dest_i;
                            break;
                        case Var_Types.DOUBLE:
                            try
                            {
                                dest_ob.Value = System.Convert.ToInt64(dest_d);
                            }
                            catch
                            {
                                //cant cast NaN to int
                                dest_ob.Value = long.MinValue;
                            }
                            break;
                    }
                    break;
                case Var_Types.DOUBLE:
                    switch (cast)
                    {
                        case Var_Types.INT:
                            dest_ob.Value = System.Convert.ToDouble(dest_i);
                            break;
                        case Var_Types.DOUBLE:
                            dest_ob.Value = dest_d;
                            break;
                    }
                    break;
                case Var_Types.STRING:
                    switch (cast)
                    {
                        case Var_Types.INT:
                            dest_ob.Value = System.Convert.ToString(dest_i);
                            break;
                        case Var_Types.DOUBLE:
                            dest_ob.Value = System.Convert.ToString(dest_d);
                            break;
                        case Var_Types.STRING:
                            dest_ob.Value = dest_str;
                            break;
                    }
                    break;
                case Var_Types.ASSIGNABLE:
                    switch (cast)
                    {
                        case Var_Types.INT:
                            dest_ob.Value = dest_i;
                            dest_ob.Type = Var_Types.INT;
                            break;
                        case Var_Types.DOUBLE:
                            dest_ob.Value = dest_d;
                            dest_ob.Type = Var_Types.DOUBLE;
                            break;
                        case Var_Types.STRING:
                            dest_ob.Value = dest_str;
                            dest_ob.Type = Var_Types.STRING;
                            break;
                    }
                    break;
                default:
                    //script error
                    Script_Error("couldn't cast type " + cast.ToString() + " to type " + dest_ob.Type.ToString());
                    break;
            }
        }


        private bool isOp(string token)
        {
            if (isUnaryOp(token) || isBinaryOp(token) || isParan(token))
            {
                return true;
            }
            return false;
        }

        private bool isParan(string token)
        {
            switch (token)
            {
                case "(":
                case ")":
                    return true;
            }

            return false;
        }

        private bool isUnaryOp(string token)
        {
            switch (token)
            {
                case "IS_ITEM_EQUIPPED":
                case "IS_AUGMENT_EQUIPPED":
                case "IS_SA_EQUIPPED":
                case "IS_READY":
                case "IS_SHOP":
                case "IS_NOBLE":
                case "IS_HERO":
                case "IS_DUELING":
                case "IS_FAKEDEATH":
                case "IS_INVISIBLE":
                case "IS_INCOMBAT":
                case "IS_SITTING":
                case "IS_WALKING":
                case "IS_FISHING":
                case "IS_DEMONSWORD":
                case "IS_TRANSFORMED":
                case "IS_AGATHON":
                case "IS_FINDPARTY":
                case "IS_USINGCUBIC":
                case "IS_FLAGGED":
                case "IS_RED":
                case "IS_PARTYMEMBER":
                case "IS_PARTYLEADER":
                case "IS_INPARTY":
                case "IS_CLANMEMBER":
                case "IS_LEADER":
                case "IS_CLANMATE":
                case "IS_INSIEGE":
                case "IS_SIEGEATTACKER":
                case "IS_SIEGEALLY":
                case "IS_SIEGEENEMY":
                case "IS_MUTUALWAR":
                case "IS_ONESIDEDWAR":
                case "IS_ALLYMEMBER":
                case "IS_TWAR":
                case "IS_BLEEDING":
                case "IS_POISONED":
                case "IS_REDCIRCLE":
                case "IS_ICE":
                case "IS_WIND":
                case "IS_AFRAID":
                case "IS_STUNNED":
                case "IS_ASLEEP":
                case "IS_MUTE":
                case "IS_ROOTED":
                case "IS_PARALYZED":
                case "IS_PETRIFIED":
                case "IS_BURNING":
                case "IS_FLOATING_ROOT":
                case "IS_DANCE_STUNNED":
                case "IS_FIREROOT_STUN":
                case "IS_STEALTH":
                case "IS_IMPRISIONING_1":
                case "IS_IMPRISIONING_2":
                case "IS_SOE":
                case "IS_ICE2":
                case "IS_EARTHQUAKE":
                case "IS_INVULNERABLE":
                case "IS_REGEN_VITALITY":
                case "IS_REAL_TARGETED":
                case "IS_DEATH_MARKED":
                case "IS_TERRIFIED":
                case "IS_CONFUSED":
                case "IS_INVINCIBLE":
                case "IS_AIR_STUN":
                case "IS_AIR_ROOT":
                case "IS_STIGMAED":
                case "IS_STAKATOROOT":
                case "IS_FREEZING":
                case "IS_DISABLED":
                case "IN_POLY":
                case "SQRT":
                case "ABS":
                case "SIN":
                case "SINH":
                case "ASIN":
                case "COS":
                case "COSH":
                case "ACOS":
                case "TAN":
                case "TANH":
                case "ATAN":
                case "~"://-
                case "VARIABLE_DEFINED":
                case "IS_DEFINED":
                case "[]":
                case "??":
                case "IS_RUNNING":
                case "IS_NOT_RUNNING":
                case "FILE_EXISTS":
                case "?I":
                case "?D":
                case "?$":
                case "?S":
                case "?A":
                case "?ST":
                case "?Q":
                case "?FR":
                case "?FW":
                case "?B":
                case "?W":
                case "?C":
                case "!":
                    return true;
            }

            return false;
        }

        private bool isBinaryOp(string token)
        {
            switch (token)
            {
                case "GROUP_HP":
                case "GROUP_MP":
                case "GROUP_CP":
                case "IS_RESISTED":
                case "IN_RANGE":
                case "IS_CLAN":
                case "IS_ALLY":
                case "IS_CLASS":
                case "IS_ROOT_CLASS":
                case "+":
                case "-":
                case "*":
                case "/":
                case "^":
                case "%":
                case "LOG":
                case "||":
                case "OR":
                case "&&":
                case "AND":
                case "|":
                case "&":
                case ">>":
                case "<<":
                case "XOR":
                case "==":
                case "!=":
                case "<":
                case "<=":
                case ">":
                case ">=":
                    return true;
            }

            return false;
        }

        private bool isFunction(string token)
        {
            return false;
        }
    }
}
