using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicClause
{
    class Program
    {
        //Xóa dấu ngoặc đơn
        //4 trường hợp: (-b); (-(a.b)); (a.b); b; => -b; -(a.b); a.b; b;
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

            //Tìm kiếm vị trí dấu "->"===================================================
            int index = c1.IndexOf('>');

            //Tách a và b từ c1
            a = c1.Substring(0, index);
            b = c1.Substring(index + 1, c1.Length - index - 1);

            //Xóa dấu ngoặc đơn
            a = removeParentheses(a);
            b = removeParentheses(b);

            //Trả về kết quả
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
        static public string MT(string c1, string c2)
        {
            string result = "", a = "", b = "";

            //Tìm kiếm vị trí dấu "->"===================================================
            int index = c1.IndexOf('>');

            //Tách a và b từ c1
            a = c1.Substring(0, index);
            b = c1.Substring(index + 1, c1.Length - index - 1);

            //Xóa dấu ngoặc đơn
            a = removeParentheses(a);
            b = removeParentheses(b);

            //Xóa dấu "-" của b
            if (b[0] == '-')
            {
                b = b.Replace("-", string.Empty);
                b = removeParentheses(b);
            }
            else if(c2[0] == '-') //hoặc xóa dấu "-" của c2
            {
                c2 = c2.Replace("-", string.Empty);
                c2 = removeParentheses(c2);
            }

            //Trả về kết quả
            if (b == c2)
            {
                if (a.Length == 1) //a
                {
                    result = "-" + a; //-a
                }
                else
                {
                    if(a[0] == '-') // -(a.b)
                    {
                        result = a.Remove(0, 1); //(a.b)
                        result = removeParentheses(result); //a.b
                    }
                    else //a.b
                    {
                        result = "-(" + a + ")"; //-(a.b)
                    }
                    
                }
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

            //Tìm kiếm vị trí dấu "->" ===================================================
            int index1 = c1.IndexOf('>');
            int index2 = c2.IndexOf('>');

            //Tách a,b,c từ c1 và c2
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
        static string DIL(string c1, string c2, string c3)
        {
            string result = "", a = "", b = "", d = "", c = "";

            int index1 = c1.IndexOf('>');
            int index2 = c2.IndexOf('>');
            int index3 = c3.IndexOf('+');

            //Tách a,b,c,d và xóa dấu ngoặc đơn từ c1 và c2
            a = removeParentheses(c1.Substring(0, index1));
            b = removeParentheses(c1.Substring(index1 + 1, c1.Length - index1 - 1));
            c = removeParentheses(c2.Substring(0, index2));
            d = removeParentheses(c2.Substring(index2 + 1, c2.Length - index2 - 1));

            //Tách a,c và xóa ngoặc đơn từ c3
            string _a = removeParentheses(c3.Substring(0, index3));
            string _c = removeParentheses(c3.Substring(index3 + 1, c3.Length - index3 - 1));

            //Trả về kết quả
            if (a == _a && c == _c)
            {
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
            }
            return result;
        }

        //Disjunctive Syllogism
        // a+b         a+b
        // -a          -b
        //------      ------
        // b           a
        static string DS(string c1, string c2)
        {
            string result = "", a = "", b = "";
            int index = c1.IndexOf('+');

            a = c1.Substring(0, index);
            b = c1.Substring(index + 1, c1.Length - index - 1);

            string tempa = a = removeParentheses(a);
            string tempb = b = removeParentheses(b);

            /*4 trường hợp: (-b); (-(a.b)); (a.b); b; => -b; -(a.b); a.b; b;*/
            

            if (c2[0] == '-')
            {
                c2 = c2.Replace("-", string.Empty);
                c2 = removeParentheses(c2);
            }
            else
            {
                if (b[0] == '-')
                {
                    b = b.Replace("-", string.Empty);
                    b = removeParentheses(b);
                }
                if (a[0] == '-')
                {
                    a = a.Replace("-", string.Empty);
                    a = removeParentheses(a);
                }
            }

            if (b == c2)
            {
                result += tempa;
            }
            else if (a == c2)
            {
                result += tempb;
            }

            return result;
        }

        //Simplification
        // a.b             a.b
        //------(id=1)    ------(id=2)
        // a               b
        static string SIM(string c1, int id)
        {
            string result = "", a = "", b = "";

            int index = c1.IndexOf('.');

            a = c1.Substring(0, index);
            b = c1.Substring(index + 1, c1.Length - index - 1);

            a = removeParentheses(a);
            b = removeParentheses(b);

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
            string a = "(-(a.b))>(-(c.d))";
            string b = "c.(d+e)";
            //string c = "(a.b)+m";
            //string tempa = a;
            //int length = b.Length;
            //for(int i = 0; i < length; i++)
            //{
            //    int temp = a.LastIndexOf(b);
            //    if (temp != -1)
            //    {
            //        Console.WriteLine(tempa.Substring(temp,tempa.Length - temp));
            //        break;
            //    }
            //    if (b.Length >= 1)
            //    {
            //        b = b.Substring(0, b.Length - 1);
            //    }
            //}
            //string result = DIL(a, b, c);
            string result = SIM(b,2);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
