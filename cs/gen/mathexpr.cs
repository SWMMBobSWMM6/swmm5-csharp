// Generated from EPA SWMM 5.2.4 (mathexpr.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int Err = 0;
    public static int Bc = 0;
    public static int PrevLex = 0;
    public static int CurLex = 0;
    public static int Len = 0;
    public static int Pos = 0;
    public static CPtr S_ = CPtr.Null;
    public static CPtr Token = R.Buf(255);
    public static int Ivar = 0;
    public static double Fvalue = 0.0;
    public static CPtr[] MathFunc = new CPtr[] { S.L1004, S.L1005, S.L1006, S.L1007, S.L1008, S.L1009, S.L1010, S.L1011, S.L739, S.L1012, S.L1013, S.L1014, S.L1015, S.L1016, S.L1017, S.L1018, S.L1019, S.L1020, S.L1021, CPtr.Null };
    public static FP3 mathexpr_getVariableIndex = null;

    // mathexpr.c:109
    public static int sametext(CPtr s1, CPtr s2)
    {
        int i = 0;
        for (i = 0; R.toupper(s1[i]) == R.toupper(s2[i]); i++)
        {
            if ((!(s1[i + 1] != 0)) && (!(s2[i + 1] != 0)))
            {
                return (1);
            }
        }
        return (0);
    }

    // mathexpr.c:130
    public static int isDigit(int c)
    {
        if ((c >= '1') && (c <= '9'))
        {
            return 1;
        }
        if (c == '0')
        {
            return 1;
        }
        return 0;
    }

    // mathexpr.c:139
    public static int isLetter(int c)
    {
        if ((c >= 'a') && (c <= 'z'))
        {
            return 1;
        }
        if ((c >= 'A') && (c <= 'Z'))
        {
            return 1;
        }
        if (c == '_')
        {
            return 1;
        }
        return 0;
    }

    // mathexpr.c:149
    public static void getToken()
    {
        CPtr c = R.BufFrom(S.L33, 2);
        S.Token[0] = 0;
        while ((S.Pos <= S.Len) && ((((S.isLetter(S.S_[S.Pos]) != 0)) || ((S.isDigit(S.S_[S.Pos]) != 0)))))
        {
            c[0] = S.S_[S.Pos];
            R.strcat(S.Token, c);
            S.Pos++;
        }
        S.Pos--;
    }

    // mathexpr.c:165
    public static int getMathFunc()
    {
        int i = 0;
        while (!S.MathFunc[i].IsNull)
        {
            if ((S.sametext(S.MathFunc[i], S.Token) != 0))
            {
                return i + 10;
            }
            i++;
        }
        return (0);
    }

    // mathexpr.c:178
    public static int getVariable()
    {
        if (!(S.mathexpr_getVariableIndex != null))
        {
            return 0;
        }
        S.Ivar = S.mathexpr_getVariableIndex(S.Token);
        if (S.Ivar >= 0)
        {
            return 8;
        }
        return 0;
    }

    // mathexpr.c:188
    public static double getNumber()
    {
        CPtr c = R.BufFrom(S.L33, 2);
        CPtr sNumber = R.Buf(255);
        int errflag = 0;
        int decimalCount = 0;
        sNumber[0] = 0;
        while ((S.Pos < S.Len) && ((S.isDigit(S.S_[S.Pos]) != 0)))
        {
            c[0] = S.S_[S.Pos];
            R.strcat(sNumber, c);
            S.Pos++;
        }
        if (S.Pos < S.Len)
        {
            if (S.S_[S.Pos] == '.')
            {
                decimalCount++;
                if (decimalCount > 1)
                {
                    S.Err = 1;
                }
                R.strcat(sNumber, S.L315);
                S.Pos++;
                while ((S.Pos < S.Len) && ((S.isDigit(S.S_[S.Pos]) != 0)))
                {
                    c[0] = S.S_[S.Pos];
                    R.strcat(sNumber, c);
                    S.Pos++;
                }
            }
            if ((S.Pos < S.Len) && (((S.S_[S.Pos] == 'e') || (S.S_[S.Pos] == 'E'))))
            {
                R.strcat(sNumber, S.L316);
                S.Pos++;
                if (S.Pos >= S.Len)
                {
                    errflag = 1;
                }
                else
                {
                    if ((S.S_[S.Pos] == '-') || (S.S_[S.Pos] == '+'))
                    {
                        c[0] = S.S_[S.Pos];
                        R.strcat(sNumber, c);
                        S.Pos++;
                    }
                    if ((S.Pos >= S.Len) || (!((S.isDigit(S.S_[S.Pos]) != 0))))
                    {
                        errflag = 1;
                    }
                    else
                    {
                        while ((S.Pos < S.Len) && ((S.isDigit(S.S_[S.Pos]) != 0)))
                        {
                            c[0] = S.S_[S.Pos];
                            R.strcat(sNumber, c);
                            S.Pos++;
                        }
                    }
                }
            }
        }
        S.Pos--;
        if ((errflag != 0))
        {
            return 0;
        }
        else
        {
            return R.atof(sNumber);
        }
    }

    // mathexpr.c:252
    public static int getOperand()
    {
        int code = 0;
        switch (S.S_[S.Pos])
        {
            case '(':
                code = 1;
                break;
            case ')':
                code = 2;
                break;
            case '+':
                code = 3;
                break;
            case '-':
                code = 4;
                if (((S.Pos < S.Len - 1) && ((S.isDigit(S.S_[S.Pos + 1]) != 0))) && (((S.CurLex <= 6) || (S.CurLex == 31))))
                {
                    S.Pos++;
                    S.Fvalue = -(S.getNumber());
                    code = 7;
                }
                break;
            case '*':
                code = 5;
                break;
            case '/':
                code = 6;
                break;
            case '^':
                code = 31;
                break;
            default:
                code = 0;
                break;
        }
        return code;
    }

    // mathexpr.c:280
    public static int getLex()
    {
        int n = 0;
        while ((S.Pos < S.Len) && (S.S_[S.Pos] == ' '))
        {
            S.Pos++;
        }
        if (S.Pos >= S.Len)
        {
            return 0;
        }
        n = S.getOperand();
        if (n == 0)
        {
            if ((S.isLetter(S.S_[S.Pos]) != 0))
            {
                S.getToken();
                n = S.getMathFunc();
                if (n == 0)
                {
                    n = S.getVariable();
                }
            }
            else if ((S.S_[S.Pos] == '.') || ((S.isDigit(S.S_[S.Pos]) != 0)))
            {
                n = 7;
                S.Fvalue = S.getNumber();
            }
        }
        S.Pos++;
        S.PrevLex = S.CurLex;
        S.CurLex = n;
        return n;
    }

    // mathexpr.c:314
    public static ExprTree newNode()
    {
        ExprTree node = null;
        node = new ExprTree();
        if (!(node != null))
        {
            S.Err = 2;
        }
        else
        {
            node.opcode = 0;
            node.ivar = -1;
            node.fvalue = 0.0;
            node.left = (null);
            node.right = (null);
        }
        return node;
    }

    // mathexpr.c:332
    public static ExprTree getSingleOp(ref int lex)
    {
        int opcode = 0;
        ExprTree left = null;
        ExprTree node = null;
        if (lex == 1)
        {
            S.Bc++;
            left = S.getTree();
        }
        else
        {
            if (((lex < 7) || (lex == 9)) || (lex > 30))
            {
                S.Err = 1;
                return (null);
            }
            opcode = lex;
            if ((lex == 7) || (lex == 8))
            {
                left = S.newNode();
                left.opcode = opcode;
                if (lex == 7)
                {
                    left.fvalue = S.Fvalue;
                }
                if (lex == 8)
                {
                    left.ivar = S.Ivar;
                }
            }
            else
            {
                lex = S.getLex();
                if (lex != 1)
                {
                    S.Err = 1;
                    return (null);
                }
                S.Bc++;
                left = S.newNode();
                left.left = S.getTree();
                left.opcode = opcode;
            }
        }
        lex = S.getLex();
        if (lex == 31)
        {
            node = S.newNode();
            node.left = left;
            node.opcode = lex;
            lex = S.getLex();
            node.right = S.getSingleOp(ref lex);
            left = node;
        }
        return left;
    }

    // mathexpr.c:397
    public static ExprTree getOp(ref int lex)
    {
        int opcode = 0;
        ExprTree left = null;
        ExprTree right = null;
        ExprTree node = null;
        int neg = 0;
        lex = S.getLex();
        if ((S.PrevLex == 0) || (S.PrevLex == 1))
        {
            if (lex == 4)
            {
                neg = 1;
                lex = S.getLex();
            }
            else if (lex == 3)
            {
                lex = S.getLex();
            }
        }
        left = S.getSingleOp(ref lex);
        while ((lex == 5) || (lex == 6))
        {
            opcode = lex;
            lex = S.getLex();
            right = S.getSingleOp(ref lex);
            node = S.newNode();
            if ((S.Err != 0))
            {
                return (null);
            }
            node.left = left;
            node.right = right;
            node.opcode = opcode;
            left = node;
        }
        if ((neg != 0))
        {
            node = S.newNode();
            if ((S.Err != 0))
            {
                return (null);
            }
            node.left = left;
            node.right = (null);
            node.opcode = 9;
            left = node;
        }
        return left;
    }

    // mathexpr.c:442
    public static ExprTree getTree()
    {
        int lex = 0;
        int opcode = 0;
        ExprTree left = null;
        ExprTree right = null;
        ExprTree node = null;
        left = S.getOp(ref lex);
        for (; ; )
        {
            if ((lex == 0) || (lex == 2))
            {
                if (lex == 2)
                {
                    S.Bc--;
                }
                break;
            }
            if ((lex != 3) && (lex != 4))
            {
                S.Err = 1;
                break;
            }
            opcode = lex;
            right = S.getOp(ref lex);
            node = S.newNode();
            if ((S.Err != 0))
            {
                break;
            }
            node.left = left;
            node.right = right;
            node.opcode = opcode;
            left = node;
        }
        return left;
    }

    // mathexpr.c:479
    public static void traverseTree(ExprTree tree, ref MathExpr expr)
    {
        MathExpr node = null;
        if (tree == null)
        {
            return;
        }
        S.traverseTree(tree.left, ref expr);
        S.traverseTree(tree.right, ref expr);
        node = new MathExpr();
        if ((node != null))
        {
            node.fvalue = tree.fvalue;
            node.opcode = tree.opcode;
            node.ivar = tree.ivar;
            node.next = (null);
            node.prev = (expr);
        }
        if ((expr != null))
        {
            (expr).next = node;
        }
        expr = node;
    }

    // mathexpr.c:501
    public static void deleteTree(ExprTree tree)
    {
        if ((tree != null))
        {
            if ((tree.left != null))
            {
                S.deleteTree(tree.left);
            }
            if ((tree.right != null))
            {
                S.deleteTree(tree.right);
            }
            R.free(tree);
        }
    }

    // mathexpr.c:516
    public static double mathexpr_eval(MathExpr expr, FP4 getVariableValue)
    {
        double[] ExprStack = new double[1024];
        MathExpr node = expr;
        double r1 = 0.0;
        double r2 = 0.0;
        int stackindex = 0;
        ExprStack[0] = 0.0;
        while ((node != null) && (stackindex >= 0))
        {
            switch (node.opcode)
            {
                case 3:
                    r1 = ExprStack[stackindex];
                    stackindex--;
                    if (stackindex < 0)
                    {
                        break;
                    }
                    r2 = ExprStack[stackindex];
                    ExprStack[stackindex] = r2 + r1;
                    break;
                case 4:
                    r1 = ExprStack[stackindex];
                    stackindex--;
                    if (stackindex < 0)
                    {
                        break;
                    }
                    r2 = ExprStack[stackindex];
                    ExprStack[stackindex] = r2 - r1;
                    break;
                case 5:
                    r1 = ExprStack[stackindex];
                    stackindex--;
                    if (stackindex < 0)
                    {
                        break;
                    }
                    r2 = ExprStack[stackindex];
                    ExprStack[stackindex] = r2 * r1;
                    break;
                case 6:
                    r1 = ExprStack[stackindex];
                    stackindex--;
                    if (stackindex < 0)
                    {
                        break;
                    }
                    r2 = ExprStack[stackindex];
                    ExprStack[stackindex] = r2 / r1;
                    break;
                case 7:
                    stackindex++;
                    if (stackindex >= 1024)
                    {
                        break;
                    }
                    ExprStack[stackindex] = node.fvalue;
                    break;
                case 8:
                    if (getVariableValue != null)
                    {
                        r1 = getVariableValue(node.ivar);
                    }
                    else
                    {
                        r1 = 0.0;
                    }
                    stackindex++;
                    if (stackindex >= 1024)
                    {
                        break;
                    }
                    ExprStack[stackindex] = r1;
                    break;
                case 9:
                    ExprStack[stackindex] = -ExprStack[stackindex];
                    break;
                case 10:
                    r1 = ExprStack[stackindex];
                    r2 = Math.Cos(r1);
                    ExprStack[stackindex] = r2;
                    break;
                case 11:
                    r1 = ExprStack[stackindex];
                    r2 = Math.Sin(r1);
                    ExprStack[stackindex] = r2;
                    break;
                case 12:
                    r1 = ExprStack[stackindex];
                    r2 = Math.Tan(r1);
                    ExprStack[stackindex] = r2;
                    break;
                case 13:
                    r1 = ExprStack[stackindex];
                    if (r1 == 0.0)
                    {
                        r2 = 0.0;
                    }
                    else
                    {
                        r2 = 1.0 / Math.Tan(r1);
                    }
                    ExprStack[stackindex] = r2;
                    break;
                case 14:
                    r1 = ExprStack[stackindex];
                    r2 = Math.Abs(r1);
                    ExprStack[stackindex] = r2;
                    break;
                case 15:
                    r1 = ExprStack[stackindex];
                    if (r1 < 0.0)
                    {
                        r2 = -1.0;
                    }
                    else if (r1 > 0.0)
                    {
                        r2 = 1.0;
                    }
                    else
                    {
                        r2 = 0.0;
                    }
                    ExprStack[stackindex] = r2;
                    break;
                case 16:
                    r1 = ExprStack[stackindex];
                    if (r1 < 0.0)
                    {
                        r2 = 0.0;
                    }
                    else
                    {
                        r2 = Math.Sqrt(r1);
                    }
                    ExprStack[stackindex] = r2;
                    break;
                case 17:
                    r1 = ExprStack[stackindex];
                    if (r1 <= 0)
                    {
                        r2 = 0.0;
                    }
                    else
                    {
                        r2 = Math.Log(r1);
                    }
                    ExprStack[stackindex] = r2;
                    break;
                case 18:
                    r1 = ExprStack[stackindex];
                    r2 = Math.Exp(r1);
                    ExprStack[stackindex] = r2;
                    break;
                case 19:
                    r1 = ExprStack[stackindex];
                    r2 = Math.Asin(r1);
                    ExprStack[stackindex] = r2;
                    break;
                case 20:
                    r1 = ExprStack[stackindex];
                    r2 = Math.Acos(r1);
                    ExprStack[stackindex] = r2;
                    break;
                case 21:
                    r1 = ExprStack[stackindex];
                    r2 = Math.Atan(r1);
                    ExprStack[stackindex] = r2;
                    break;
                case 22:
                    r1 = ExprStack[stackindex];
                    r2 = 1.57079632679489661923 - Math.Atan(r1);
                    ExprStack[stackindex] = r2;
                    break;
                case 23:
                    r1 = ExprStack[stackindex];
                    r2 = (Math.Exp(r1) - Math.Exp(-r1)) / 2.0;
                    ExprStack[stackindex] = r2;
                    break;
                case 24:
                    r1 = ExprStack[stackindex];
                    r2 = (Math.Exp(r1) + Math.Exp(-r1)) / 2.0;
                    ExprStack[stackindex] = r2;
                    break;
                case 25:
                    r1 = ExprStack[stackindex];
                    r2 = (Math.Exp(r1) - Math.Exp(-r1)) / (Math.Exp(r1) + Math.Exp(-r1));
                    ExprStack[stackindex] = r2;
                    break;
                case 26:
                    r1 = ExprStack[stackindex];
                    r2 = (Math.Exp(r1) + Math.Exp(-r1)) / (Math.Exp(r1) - Math.Exp(-r1));
                    ExprStack[stackindex] = r2;
                    break;
                case 27:
                    r1 = ExprStack[stackindex];
                    if (r1 == 0.0)
                    {
                        r2 = 0.0;
                    }
                    else
                    {
                        r2 = Math.Log10(r1);
                    }
                    ExprStack[stackindex] = r2;
                    break;
                case 28:
                    r1 = ExprStack[stackindex];
                    if (r1 <= 0.0)
                    {
                        r2 = 0.0;
                    }
                    else
                    {
                        r2 = 1.0;
                    }
                    ExprStack[stackindex] = r2;
                    break;
                case 31:
                    r1 = ExprStack[stackindex];
                    stackindex--;
                    if (stackindex < 0)
                    {
                        break;
                    }
                    r2 = ExprStack[stackindex];
                    if (r2 <= 0.0)
                    {
                        r2 = 0.0;
                    }
                    else
                    {
                        r2 = Math.Pow(r2, r1);
                    }
                    ExprStack[stackindex] = r2;
                    break;
            }
            node = node.next;
        }
        if (stackindex >= 0)
        {
            r1 = ExprStack[stackindex];
        }
        else
        {
            r1 = 0.0;
        }
        if (r1 != r1)
        {
            r1 = 0.0;
        }
        return r1;
    }

    // mathexpr.c:735
    public static void mathexpr_delete(MathExpr expr)
    {
        if ((expr != null))
        {
            S.mathexpr_delete(expr.next);
        }
        R.free(expr);
    }

    // mathexpr.c:743
    public static MathExpr mathexpr_create(CPtr formula, FP3 getVar)
    {
        ExprTree tree = null;
        MathExpr expr = (null);
        MathExpr result = (null);
        S.mathexpr_getVariableIndex = getVar;
        S.Err = 0;
        S.PrevLex = 0;
        S.CurLex = 0;
        S.S_ = formula;
        S.Len = (int)(R.strlen(S.S_));
        S.Pos = 0;
        S.Bc = 0;
        tree = S.getTree();
        if ((S.Bc == 0) && (S.Err == 0))
        {
            S.traverseTree(tree, ref expr);
            while ((expr != null))
            {
                result = expr;
                expr = expr.prev;
            }
        }
        S.deleteTree(tree);
        return result;
    }

}
}
