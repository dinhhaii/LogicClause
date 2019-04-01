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
            int firstIndex = a.IndexOf('('); //Vị trí dấu ngoặc đầu tiên
            int lastIndex = a.LastIndexOf(')'); //Vị trí dấu ngoặc cuối cùng

            if (firstIndex != -1 && lastIndex != -1)
            {
                int[] openParentheses = new int[10]; //Danh sách vị trí dấu mở ngoặc '('
                int[] closeParentheses = new int[10]; //Danh sách vị trí dấu đóng ngoặc ')'
                int count = 0; //Số lượng dấu ngoặc '(' hoặc ')'
                int flag = 0; //Đánh dấu vị trí dấu ngoặc '(' chưa được sử dụng

                for(int i = firstIndex; i < a.Length; i++)
                {
                    if(a[i] == '(')
                    {
                        openParentheses[count] = i;               
                        flag = count; //Đánh dấu lại vị trí dấu ngoặc '(' cuối cùng xuất hiện
                        count++;
                    }
                    else if(a[i] == ')')
                    {
                        while(closeParentheses[flag] != 0) //Chọn vị trí dấu ngoặc ')' ứng với dấu '(' tiếp theo
                        {
                            flag--;
                        }
                        closeParentheses[flag] = i;
                    }
                }

                if(closeParentheses[0] == lastIndex && firstIndex == 0)
                {
                    a = a.Remove(firstIndex, 1);
                    a = a.Remove(lastIndex-1, 1);
                }                
            }

            return a;
        }

        //Tìm và xuất vị trí của kí tự trong chuỗi
        static List<int> findCharacter(string str, char c)
        {
            List<int> result = new List<int>();
            for(int i = 0; i < str.Length; i++)
            {
                if(str[i] == c)
                {
                    result.Add(i);
                }
            }
            
            return result;
        }

        //Đếm số lượng kí tự 'character' xuất hiện trong chuỗi
        static int countCharacter(string str, char character)
        {
            int count = 0;
            for(int i = 0; i < str.Length; i++)
            {
                if(character == str[i])
                {
                    count++;
                }
            }
            return count;
        }

        // Modus Ponens
        // a->b
        // a
        //------
        // b
        static string MP(string c1, string c2)
        {
            string result = "", a = "", b = "";

            //Tìm kiếm vị trí dấu "->"
            List<int> indexList = findCharacter(c1, '>');
            for (int i = 0; i < indexList.Count; i++)
            {
                int index = indexList[i];
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
                if (result != "")
                {
                    return result;
                }
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

            //Tìm kiếm vị trí dấu "->"
            List<int> indexList = findCharacter(c1, '>');
            for (int i = 0; i < indexList.Count; i++)
            {
                int index = indexList[i];
                //Tách a và b từ c1
                a = c1.Substring(0, index);
                b = c1.Substring(index + 1, c1.Length - index - 1);

                //Xóa dấu ngoặc đơn
                a = removeParentheses(a);
                b = removeParentheses(b);

                //Kiểm tra b và c2 có dấu trừ ở đầu
                bool isNegative = false;

                //Xóa dấu "-" của b
                if (b[0] == '-')
                {
                    b = b.Replace("-", string.Empty);
                    b = removeParentheses(b);
                    isNegative = true;
                }
                else if (c2[0] == '-') //hoặc xóa dấu "-" của c2
                {
                    c2 = c2.Replace("-", string.Empty);
                    c2 = removeParentheses(c2);
                    isNegative = true;
                }

                //Trả về kết quả
                if (b == c2 && isNegative)
                {
                    if (a.Length == 1) //a
                    {
                        result = "-" + a; //-a
                    }
                    else
                    {
                        if (a[0] == '-') // -(a.b)
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
                if (result != "")
                {
                    return result;
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

            //Tìm kiếm vị trí dấu "->" 

            List<int> indexList1 = findCharacter(c1, '>');
            List<int> indexList2 = findCharacter(c2, '>');

            for(int i = 0; i < indexList1.Count; i++)
            {
                for(int j = 0; j < indexList2.Count; j++)
                {
                    int index1 = indexList1[i];
                    int index2 = indexList2[j];

                    //Tách a,b,c từ c1 và c2
                    a = c1.Substring(0, index1);
                    b1 = c1.Substring(index1 + 1, c1.Length - index1 - 1);
                    b2 = c2.Substring(0, index2);
                    c = c2.Substring(index2 + 1, c2.Length - index2 - 1);

                    if (b1 == b2)
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

                    if(result != "")
                    {
                        return result;
                    }
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

            if (c1 != c2)
            {
                if (c1.Length == 1)
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

            List<int> indexList1 = findCharacter(c1, '>');
            List<int> indexList2 = findCharacter(c2, '>');
            List<int> indexList3 = findCharacter(c3, '+');

            for (int i = 0; i < indexList1.Count; i++)
            {
                for (int j = 0; j < indexList2.Count; j++)
                {
                    for (int k = 0; k < indexList3.Count; k++)
                    {
                        int index1 = indexList1[i];
                        int index2 = indexList2[j];
                        int index3 = indexList3[k];
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
                        if(result != "")
                        {
                            return result;
                        }
                    }
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

            List<int> indexList = findCharacter(c1, '+');
            for (int i = 0; i < indexList.Count; i++)
            {
                int index = indexList[i];

                a = c1.Substring(0, index);
                b = c1.Substring(index + 1, c1.Length - index - 1);

                string tempa = a = removeParentheses(a);
                string tempb = b = removeParentheses(b);

                /*4 trường hợp: (-b); (-(a.b)); (a.b); b; => -b; -(a.b); a.b; b;*/

                if (c2[0] == '-')
                {
                    c2 = c2.Replace("-", string.Empty);
                    c2 = removeParentheses(c2);
                    //So sánh kết quả
                    if (b == c2)
                    {
                        result += tempa;
                    }
                    else if (a == c2)
                    {
                        result += tempb;
                    }
                }
                else if(b[0] == '-' || a[0] == '-')
                {
                    if (b[0] == '-')
                    {
                        b = b.Replace("-", string.Empty);
                        b = removeParentheses(b);
                        //So sánh kết quả
                        if (b == c2)
                        {
                            result += tempa;
                        }
                    }
                    if (a[0] == '-')
                    {
                        a = a.Replace("-", string.Empty);
                        a = removeParentheses(a);
                        //So sánh kết quả
                        if (a == c2)
                        {
                            result += tempb;
                        }
                    }
                }

                if(result != "")
                {
                    return result;
                }
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

            //Tìm vị trí xuất hiện "."
            List<int> indexList = findCharacter(c1, '.');

            for (int i = 0; i < indexList.Count; i++)
            {
                int index = indexList[i];
                a = c1.Substring(0, index);
                b = c1.Substring(index + 1, c1.Length - index - 1);

                //Kiểm tra vị trí "." có phù hợp
                if(countCharacter(a,'(') == countCharacter(a,')') && countCharacter(b, '(') == countCharacter(b, ')'))
                {
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
                    if (result != "")
                    {
                        return result;
                    }
                }               
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

        //Kiểm tra các mệnh đề có nhập đúng 
        static bool checkInput(List<string> clause)
        {
            for(int i = 0; i < clause.Count; i++)
            {
                for(int j = 0; j < clause[i].Length - 1; j++)
                {
                    switch (clause[i][j])
                    {
                        case '+':
                        case '.':
                        case '>':
                        case '-':
                            switch (clause[i][j + 1])
                            {
                                case '+':
                                case '.':
                                case '>':
                                case '-':
                                    return false;
                            }
                            break;
                    }
                }
            }
            return true;
        }

        static void handleClause(List<string> clause,string result)
        {
            if (!checkInput(clause))
            {
                Console.WriteLine("Syntax Error");
                return;
            }

            for(int i = 0; i < clause.Count; i++)
            {
                //Xử lý hàm SIM
                string SIMResult1 = "", SIMResult2 = "";
                SIMResult1 = SIM(clause[i], 1);
                SIMResult2 = SIM(clause[i], 2);

                //Nếu có kết quả
                if(SIMResult1 != "")
                {
                    if (!clause.Any(item => item == SIMResult1)) //Nếu kết quả không tồn tại trong tập các mệnh đề
                    {
                        clause.Add(SIMResult1);
                        Console.WriteLine("{0}. {1} | SIM {2}",clause.Count - 1,SIMResult1, i);
                        if (SIMResult1 == result)
                        {
                            return;
                        }
                    }
                    if (!clause.Any(item => item == SIMResult2)) //Nếu kết quả không tồn tại trong tập các mệnh đề
                    {
                        clause.Add(SIMResult2);
                        Console.WriteLine("{0}. {1} | SIM {2}", clause.Count - 1, SIMResult2,i);
                        if (SIMResult2 == result)
                        {
                            return;
                        }
                    }
                    
                }

                for (int j = 0; j < clause.Count; j++)
                {
                    string tempResult = "";
                    int select = 0;
                    //Xử lý các lệnh còn lại
                    while (tempResult == "")
                    {
                        select++;
                        switch (select)
                        {
                            case 1: tempResult = MP(clause[i], clause[j]); break;
                            case 2: tempResult = MT(clause[i], clause[j]); break;
                            case 3: tempResult = DS(clause[i], clause[j]); break;
                            case 4: //tempResult = CON(clause[i], clause[j]); 
                                break;
                            case 5: tempResult = HS(clause[i], clause[j]); break;
                            case 6: break;
                            default: tempResult = "-1"; break;
                        }
                        
                    }
                    if (tempResult != "" && tempResult != "-1")
                    {
                        //Kiểm tra kết quả có tồn tại trong các mệnh đề ban đầu
                        if (!clause.Any(item => item == tempResult))
                        {
                            clause.Add(tempResult);
                            Console.Write("{0}. {1}", clause.Count - 1, tempResult);
                            switch (select)
                            {
                                case 1: Console.WriteLine(" | MP {0},{1}", i, j); break;
                                case 2: Console.WriteLine(" | MT {0},{1}", i, j); break;
                                case 3: Console.WriteLine(" | DS {0},{1}", i, j); break;
                                case 4: //Console.WriteLine(" | CON {0},{1}", i, j);
                                    break;
                                case 5: Console.WriteLine(" | HS {0},{1}", i, j); break;
                                case 6: break;
                            }
                            if(tempResult == result)
                            {
                                return;
                            }
                            i = 0; j = 0;
                        }
                    }

                    for (int k = 0; k < clause.Count; k++)
                    {
                        string DILResult = "";
                        DILResult = DIL(clause[i], clause[j], clause[k]);
                        if (DILResult != "")
                        {
                            if (!clause.Any(item => item == DILResult))
                            {
                                clause.Add(DILResult);
                                Console.Write("{0}. {1} | DIL {2},{3},{4}",clause.Count - 1,DILResult,i,j,k);
                                i = 0; j = 0;k = 0;
                            }
                            
                        }
                        if (DILResult == result)
                        {
                            return;
                        }
                    }
                }
            }

            for(int i = 0; i < clause.Count; i++)
            {
                if(clause[i] == result)
                {
                    Console.WriteLine("Right");
                }
            }
        }

        static void Main(string[] args)
        {

            List<string> t = new List<string>();
            t.Add("A+(-B)+(-D)");
            t.Add("(E.F)>D");
            t.Add("-A");
            t.Add("E.F");

            string result = "-B";

            for (int i = 0; i < t.Count; i++)
            {
                Console.WriteLine(i.ToString() + ". " + t[i]);
            }
            Console.WriteLine("------------");

            handleClause(t, result);

            Console.ReadKey();
        }
    }
}
