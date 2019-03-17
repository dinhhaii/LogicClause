using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicClause
{
    class Program
    {
        //Xóa ngoặc đơn
        static string removeParentheses(string a)
        {
            int firstIndex = a.IndexOf('(');          
            if (firstIndex != -1)
            {
                a = a.Remove(firstIndex, 1);
            }

            int lastIndex = a.LastIndexOf(')');
            if (lastIndex != -1)
            {
                a = a.Remove(lastIndex, 1);
            }
            return a;
        }

        // Modus Ponens
        // a->b
        // a
        //------
        // b
        static string MP(string c1, string c2)
        {
            string result = "", a = "", b = "";

            int index = c1.IndexOf('>');
            a = c1.Substring(0, index);
            b = c1.Substring(index + 1, c1.Length - index - 1);

            a = removeParentheses(a);
            b = removeParentheses(b);

            if (c2 == a)
            { 
                result += b;
            }

            return result;
        }

        // Modus Tollens
        // a->b
        // -b
        //------
        // -a

        static string supportMT(string a)
        {
            string result = "";
            if (a.Length == 1)
            {
                result = "-" + a;
            }
            else
            {
                result = "-(" + a + ")";
            }
            return result;
        }

        static public string MT(string c1, string c2)
        {
            string result = "", a = "", b = "";
            int index = c1.IndexOf('>');
            a = c1.Substring(0, index);
            b = c1.Substring(index + 1, c1.Length - index - 1);

            a = removeParentheses(a);
            b = removeParentheses(b);

            if (b[0] == '-' && b[1] == '(')
            {
                b = removeParentheses(b);
                b = b.Substring(1, b.Length - 1);
                result = supportMT(a);
            }
            else if ((c2[0] == '-' && c2[1] == '(') || (c2[0] == '-'))
            {
                c2 = removeParentheses(c2);
                c2 = c2.Substring(1, c2.Length - 1);
                result = supportMT(a);
            }
            else if (b[0] == '-' && b.Length == 2)
            {
                b = b.Replace("-", string.Empty);
                result = supportMT(a);
            }
            else if (c2[0] == '-' && c2.Length == 2)
            {
                c2 = c2.Replace("-", string.Empty);
                result = supportMT(a);
            }

            return result;
        }

        // Hypothetical Syllogism
        // a->b
        // b->c
        //------
        // a->c
        static public string HS(string c1, string c2)
        {
            string result = "", a = "", b1 = "", b2 = "", c = "";
            int index1 = c1.IndexOf('>');
            int index2 = c2.IndexOf('>');

            a = c1.Substring(0, index1);
            b1 = c1.Substring(index1 + 1, c1.Length - index1 - 1);
            b2 = c2.Substring(0, index2);
            c = c2.Substring(index2 + 1, c2.Length - index2 - 1);

            if(b1 == b2)
            {
                if (a.Length == 1)
                {
                    result += a;
                }
                else
                {
                    result += "(" + a + ")";
                }

                if (c.Length == 1)
                {
                    result += ">" + c;
                }
                else
                {
                    result += ">(" + c + ")";
                }
            }

            return result;
        }

        // Conjunction
        // a
        // b
        //------
        // a.b
        static string CON(string c1, string c2)
        {
            string result = "";
            if(c1.Length == 1)
            {
                result += c1;
            }
            else
            {
                result += "(" + c1 + ")";
            }

            if (c2.Length == 1)
            {
                result += "." + c2;
            }
            else
            {
                result += ".(" + c2 + ")";
            }
            return result;
        }

        //Dilemma
        // a->b
        // c->d
        // a+c
        //------
        // b+d
        static string DIL(string a, string b, string c, string d)
        {
            string result = "";
            if (b.Length == 1)
            {
                result += b;
            }
            else
            {
                result += "(" + b + ")";
            }

            if (d.Length == 1)
            {
                result += "+" + d;
            }
            else
            {
                result += "+(" + d + ")";
            }
            return result;
        }

        //Disjunctive Syllogism
        // a+b         a+b
        // -a          -b
        //------      ------
        // b           a
        static string DS(string c1, string c2, int id)
        {
            string result = "", a = "", b = "";
            int index = c1.IndexOf('>');
            a = c1.Substring(0, index);
            b = c1.Substring(index + 1, c1.Length - index - 1);

            a = removeParentheses(a);
            b = removeParentheses(b);

            if (b[0] == '-' && b[1] == '(')
            {
                b = removeParentheses(b);
                b = b.Substring(1, b.Length - 1);
                result = supportMT(a);
            }
            else if ((c2[0] == '-' && c2[1] == '(') || (c2[0] == '-'))
            {
                c2 = removeParentheses(c2);
                c2 = c2.Substring(1, c2.Length - 1);
                result = supportMT(a);
            }
            else if (b[0] == '-' && b.Length == 2)
            {
                b = b.Replace("-", string.Empty);
                result = supportMT(a);
            }
            else if (c2[0] == '-' && c2.Length == 2)
            {
                c2 = c2.Replace("-", string.Empty);
                result = supportMT(a);
            }

            return result;
        }

        //Simplification
        // a.b             a.b
        //------(id=1)    ------(id=2)
        // a               b
        static string SIM(string a, string b, int id)
        {
            string result = "";
            if (id == 1)
            {
                result = a;
            }
            else
            {
                result = b;
            }
            return result;
        }

        //Addition
        // a    
        //------
        // a+b  
        static string ADD(string a, string b)
        {
            string result = "";
            if (a.Length == 1)
            {
                result += a;
            }
            else
            {
                result += "(" + a + ")";
            }

            if (b.Length == 1 || b[0] == '(')
            {
                result += "+" + b;
            }
            else
            {
                result += "+(" + b + ")";
            }
            return result;
        }


        static void handleClause(string[] clause,string result)
        {

        }

        static void Main(string[] args)
        {
            string a = "homnaydihocthem";
            string b = "hocthembai";
            string tempa = a;
            int length = b.Length;
            for(int i = 0; i < length; i++)
            {
                int temp = a.LastIndexOf(b);
                if (temp != -1)
                {
                    Console.WriteLine(tempa.Substring(temp,tempa.Length - temp));
                    break;
                }
                if (b.Length >= 1)
                {
                    b = b.Substring(0, b.Length - 1);
                }
            }
            //Console.WriteLine(HS(a, b));
            Console.ReadKey();
        }
    }
}
