using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicClause
{
    class Program
    {
        //Kiểm tra dấu ngoặc đơn có bao bọc cả mệnh đề (VD: ((a+b)>c) --> true | (a+b)>c --> false)
        static bool checkParentheses(string a)
        {
            int firstIndex = a.IndexOf('('); //Vị trí dấu ngoặc đầu tiên
            int lastIndex = a.LastIndexOf(')'); //Vị trí dấu ngoặc cuối cùng

            if (firstIndex != -1 && lastIndex != -1)
            {
                int[] openParentheses = new int[10]; //Danh sách vị trí dấu mở ngoặc '('
                int[] closeParentheses = new int[10]; //Danh sách vị trí dấu đóng ngoặc ')'
                int count = 0; //Số lượng dấu ngoặc '(' hoặc ')'
                int flag = 0; //Đánh dấu vị trí dấu ngoặc '(' chưa được sử dụng

                for (int i = firstIndex; i < a.Length; i++)
                {
                    if (a[i] == '(')
                    {
                        openParentheses[count] = i;
                        flag = count; //Đánh dấu lại vị trí dấu ngoặc '(' cuối cùng xuất hiện
                        count++;
                    }
                    else if (a[i] == ')')
                    {
                        while (closeParentheses[flag] != 0) //Chọn vị trí dấu ngoặc ')' ứng với dấu '(' tiếp theo
                        {
                            flag--;
                        }
                        closeParentheses[flag] = i;
                    }
                }

                if (closeParentheses[0] == lastIndex && firstIndex == 0)
                {
                    return true;
                }
            }
            return false;
        }


        //Xóa dấu ngoặc đơn
        //4 trường hợp: (-b); (-(a.b)); (a.b); b; => -b; -(a.b); a.b; b;
        static string removeParentheses(string a)
        {
            int firstIndex = a.IndexOf('('); //Vị trí dấu ngoặc đầu tiên
            int lastIndex = a.LastIndexOf(')'); //Vị trí dấu ngoặc cuối cùng

            if(checkParentheses(a)){
                a = a.Remove(firstIndex, 1);
                a = a.Remove(lastIndex - 1, 1);
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

                //Kiểm tra nếu số lượng dấu '(' = số lượng dấu ')' thì xử lý
                if (countCharacter(a, '(') == countCharacter(a, ')') && countCharacter(b, '(') == countCharacter(b, ')'))
                {

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

                //Kiểm tra nếu số lượng dấu '(' = số lượng dấu ')' thì xử lý
                if (countCharacter(a, '(') == countCharacter(a, ')') && countCharacter(b, '(') == countCharacter(b, ')'))
                {
                    //Xóa dấu ngoặc đơn
                    a = removeParentheses(a);
                    b = removeParentheses(b);

                    //Kiểm tra b và c2 có dấu trừ ở đầu
                    bool isNegative = false;

                    //Xóa dấu "-" của b
                    if (b[0] == '-')
                    {
                        b = b.Substring(1, b.Length - 1);
                        b = removeParentheses(b);
                        isNegative = true;
                    }
                    else if (c2[0] == '-') //hoặc xóa dấu "-" của c2
                    {
                        c2 = c2.Substring(1, c2.Length - 1);
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

                //Kiểm tra nếu số lượng dấu '(' = số lượng dấu ')' thì xử lý
                if (countCharacter(a, '(') == countCharacter(a, ')') && countCharacter(b, '(') == countCharacter(b, ')'))
                {
                    string tempa = a = removeParentheses(a);
                    string tempb = b = removeParentheses(b);

                    /*4 trường hợp: (-b); (-(a.b)); (a.b); b; => -b; -(a.b); a.b; b;*/

                    if (c2[0] == '-')
                    {
                        c2 = c2.Substring(1, c2.Length - 1);
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
                    else if (b[0] == '-' || a[0] == '-')
                    {
                        if (b[0] == '-')
                        {
                            b = b.Substring(1, b.Length - 1);
                            b = removeParentheses(b);
                            //So sánh kết quả
                            if (b == c2)
                            {
                                result += tempa;
                            }
                        }
                        if (a[0] == '-')
                        {
                            a = a.Substring(1, a.Length - 1);
                            a = removeParentheses(a);
                            //So sánh kết quả
                            if (a == c2)
                            {
                                result += tempb;
                            }
                        }
                    }

                    if (result != "")
                    {
                        return result;
                    }
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

        //Equivalence
        static List<string> EQ(string clause)
        {
            List<string> result = new List<string>();
            string a = "", b = "";
            List<int> indexAndOfClauseList = findCharacter(clause, '.');
            List<int> indexOrOfClauseList = findCharacter(clause, '+');

            for (int i = 0; i < indexAndOfClauseList.Count; i++)
            {
                int index = indexAndOfClauseList[i];
                //Tách a và b từ clause
                a = clause.Substring(0, index);
                b = clause.Substring(index + 1, clause.Length - index - 1);

                //Kiểm tra nếu số lượng dấu '(' = số lượng dấu ')' thì xử lý
                if (countCharacter(a, '(') == countCharacter(a, ')') && countCharacter(b, '(') == countCharacter(b, ')'))
                {
                    //1.1) a.b = b.a =================================================================================================
                    string temp = b + "." + a;
                    if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                    {
                        result.Add(temp);
                    }

                    //2.1) (a1.a2).b = a1.(a2.b) =================================================================================================
                    if (checkParentheses(a)) //Kiểm tra dấu ngoặc đơn có bao ngoài mệnh đề a
                    {
                        string tempa = removeParentheses(a);
                        List<int> indexAndList = findCharacter(tempa, '.');

                        for (int j = 0; j < indexAndList.Count; j++)
                        {
                            //Tách chuỗi a thành 2 chuỗi a1,a2
                            string tempa1 = tempa.Substring(0, indexAndList[j]);
                            string tempa2 = tempa.Substring(indexAndList[j] + 1, tempa.Length - indexAndList[j] - 1);
                            if (countCharacter(tempa1, '(') == countCharacter(tempa1, ')') && countCharacter(tempa2, '(') == countCharacter(tempa2, ')'))
                            {
                                temp = string.Format("{0}.({1}.{2})", tempa1, tempa2, b);
                                if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                                {
                                    result.Add(temp);
                                }
                            }
                        }
                    }

                    //3.1) a.(b1+b2) = (a.b1)+(a.b2) =================================================================================================
                    if (checkParentheses(b)) //Kiểm tra dấu ngoặc đơn có bao ngoài mệnh đề b
                    {
                        string tempb = removeParentheses(b);
                        List<int> indexOrList = findCharacter(tempb, '+');

                        for (int j = 0; j < indexOrList.Count; j++)
                        {
                            //Tách chuỗi b thành 2 chuỗi b1,b2
                            string tempb1 = tempb.Substring(0, indexOrList[j]);
                            string tempb2 = tempb.Substring(indexOrList[j] + 1, tempb.Length - indexOrList[j] - 1);

                            if (countCharacter(tempb1, '(') == countCharacter(tempb1, ')') && countCharacter(tempb2, '(') == countCharacter(tempb2, ')'))
                            {
                                temp = string.Format("({0}.{1})+({2}.{3})", a, tempb1, a, tempb2);
                                if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                                {
                                    result.Add(temp);
                                }
                            }
                        }
                    }

                    //7.1) a.1=a =================================================================================================
                    if (b == "1")
                    {
                        temp = removeParentheses(a);
                        if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                        {
                            result.Add(temp);
                        }
                    }

                    //8.1) a.0=0 =================================================================================================
                    if (b == "0")
                    {
                        temp = b;
                        if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                        {
                            result.Add(temp);
                        }
                    }

                    //9.1) a.(-a)=0 =================================================================================================
                    string tempRmva = removeParentheses(a);
                    string tempRmvb = removeParentheses(b);

                    if ((tempRmva[0] == '-' && removeParentheses(tempRmva.Substring(1, tempRmva.Length - 1)) == tempRmvb) || (tempRmvb[0] == '-' && removeParentheses(tempRmvb.Substring(1, tempRmvb.Length - 1)) == tempRmva)) //Kiểm tra a = -b hoặc -a = b
                    {
                        temp = "0";
                        if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                        {
                            result.Add(temp);
                        }
                    }
                }
            }

            for (int i = 0; i < indexOrOfClauseList.Count; i++)
            {
                int index = indexOrOfClauseList[i];
                //Tách a và b từ clause
                a = clause.Substring(0, index);
                b = clause.Substring(index + 1, clause.Length - index - 1);

                //Kiểm tra nếu số lượng dấu '(' = số lượng dấu ')' thì xử lý
                if (countCharacter(a, '(') == countCharacter(a, ')') && countCharacter(b, '(') == countCharacter(b, ')'))
                {
                    //1.2) a+b = b+a =================================================================================================
                    string temp = b + "+" + a;
                    if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                    {
                        result.Add(temp);
                    }

                    //2.2) (a1+a2)+b = a1+(a2+b) =================================================================================================
                    if (checkParentheses(a)) //Kiểm tra dấu ngoặc đơn có bao ngoài mệnh đề a
                    {
                        string tempa = removeParentheses(a);
                        List<int> indexAndList = findCharacter(tempa, '+');

                        for (int j = 0; j < indexAndList.Count; j++)
                        {
                            //Tách chuỗi a thành 2 chuỗi a1,a2
                            string tempa1 = tempa.Substring(0, indexAndList[j]);
                            string tempa2 = tempa.Substring(indexAndList[j] + 1, tempa.Length - indexAndList[j] - 1);
                            if (countCharacter(tempa1, '(') == countCharacter(tempa1, ')') && countCharacter(tempa2, '(') == countCharacter(tempa2, ')'))
                            {
                                temp = string.Format("{0}+({1}+{2})", tempa1, tempa2, b);
                                if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                                {
                                    result.Add(temp);
                                }
                            }
                        }
                    }

                    //3.2) a+(b1.b2) = (a+b1).(a+b2) =================================================================================================
                    if (checkParentheses(b)) //Kiểm tra dấu ngoặc đơn có bao ngoài mệnh đề b
                    {
                        string tempb = removeParentheses(b);
                        List<int> indexOrList = findCharacter(tempb, '.');

                        for (int j = 0; j < indexOrList.Count; j++)
                        {
                            //Tách chuỗi b thành 2 chuỗi b1,b2
                            string tempb1 = tempb.Substring(0, indexOrList[j]);
                            string tempb2 = tempb.Substring(indexOrList[j] + 1, tempb.Length - indexOrList[j] - 1);

                            if (countCharacter(tempb1, '(') == countCharacter(tempb1, ')') && countCharacter(tempb2, '(') == countCharacter(tempb2, ')'))
                            {
                                temp = string.Format("({0}+{1}).({2}+{3})", a, tempb1, a, tempb2);
                                if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                                {
                                    result.Add(temp);
                                }
                            }
                        }
                    }

                    //7.2) a+1=1 =================================================================================================
                    if (b == "1")
                    {
                        temp = "1";
                        if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                        {
                            result.Add(temp);
                        }
                    }

                    //8.2) a+0=a =================================================================================================
                    if (b == "0")
                    {
                        temp = a;
                        if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                        {
                            result.Add(temp);
                        }
                    }

                    //9.2) a+(-a)=1 =================================================================================================
                    string tempRmva = removeParentheses(a);
                    string tempRmvb = removeParentheses(b);

                    if ((tempRmva[0] == '-' && removeParentheses(tempRmva.Substring(1, tempRmva.Length - 1)) == tempRmvb) || (tempRmvb[0] == '-' && removeParentheses(tempRmvb.Substring(1, tempRmvb.Length - 1)) == tempRmva)) //Kiểm tra a = -b hoặc -a = b
                    {
                        temp = "1";
                        if (!result.Any(item => item == temp)) //Kiểm tra xem đã tồn tại 
                        {
                            result.Add(temp);
                        }
                    }
                }
            }

            //5.1) -(-a)=a =================================================================================================
            if (clause.IndexOf("-(-") == 0 || clause.IndexOf("--") == 0)
            {
                string temp = "";
                if (clause.IndexOf("-(-") == 0)
                {
                    temp = clause.Substring(3, clause.Length - 3);
                }
                else if (clause.IndexOf("--") == 0)
                {
                    temp = clause.Substring(2, clause.Length - 2);
                }
                if(!result.Any(item => item == temp) && temp != "")
                {
                    result.Add(temp);
                }
            }

            
            if (clause.IndexOf('-') == 0)
            {  
                string temp = "";
                //Loại bỏ dấu trừ và dấu ngoặc
                string tempa = clause.Substring(1, clause.Length - 1);
                tempa = removeParentheses(tempa);
                List<int> indexAndList = findCharacter(tempa,'.');
                List<int> indexOrList = findCharacter(tempa, '+');

                //index - Lưu vị trí được tách thành các chuỗi 
                int lengthStrAnd = 0;
                int lengthStrOr = 0;

                //Mảng chuỗi kết quả
                Stack<string> resultSplitAnd = new Stack<string>();
                Stack<string> resultSplitOr = new Stack<string>();

                //5.2) -(a.b) = (-a)+(-b) =================================================================================================
                //Duyệt các kí tự '.' được tìm thấy
                for (int i = 0; i < indexAndList.Count; i++)
                {
                    //Tách 2 chuỗi tại vị trí dấu '.'
                    string splitStr1 = tempa.Substring(0, indexAndList[i] - lengthStrAnd);
                    string splitStr2 = tempa.Substring(indexAndList[i] - lengthStrAnd + 1, tempa.Length - indexAndList[i] + lengthStrAnd - 1);

                    if (countCharacter(splitStr1, '(') == countCharacter(splitStr1, ')') && countCharacter(splitStr2, '(') == countCharacter(splitStr2, ')'))
                    {
                        lengthStrAnd = splitStr1.Length + 1;

                        //Xóa ngoặc đơn
                        if (splitStr1 != removeParentheses(splitStr1))
                        {
                            splitStr1 = removeParentheses(splitStr1);
                            lengthStrAnd += 2;
                        }
                        if (splitStr2 != removeParentheses(splitStr2))
                        {
                            splitStr2 = removeParentheses(splitStr2);
                            lengthStrAnd += 2;
                        }

                        //Lấy phần tử được tách chuỗi trước đó 
                        if(resultSplitAnd.Count != 0)
                        {
                            resultSplitAnd.Pop();
                        }
                        
                        //Thêm các phần tử mới được tách
                        resultSplitAnd.Push(splitStr1);
                        resultSplitAnd.Push(splitStr2);

                        tempa = splitStr2;
                        
                    }
                }
                foreach(var item in resultSplitAnd)
                {
                    if(item.Length!= 1)
                    {
                        temp = "(-(" + item + "))+" + temp;
                    }
                    else
                    {
                        temp = "(-" + item + ")+" + temp;
                    }
                }

                if (temp != "")
                {
                    temp = temp.Substring(0, temp.Length - 1);

                    if (!result.Any(item => item == temp))
                    {
                        result.Add(temp);
                    }
                }

                //5.3) -(a+b) = (-a).(-b) =================================================================================================
                //Duyệt các kí tự '+' được tìm thấy
                for (int i = 0; i < indexOrList.Count; i++)
                {
                    //Tách 2 chuỗi tại vị trí dấu '+'
                    string splitStr1 = tempa.Substring(0, indexOrList[i] - lengthStrOr);
                    string splitStr2 = tempa.Substring(indexOrList[i] - lengthStrOr + 1, tempa.Length - indexOrList[i] + lengthStrOr - 1);

                    if (countCharacter(splitStr1, '(') == countCharacter(splitStr1, ')') && countCharacter(splitStr2, '(') == countCharacter(splitStr2, ')'))
                    {
                        lengthStrOr = splitStr1.Length + 1;

                        //Xóa ngoặc đơn
                        if (splitStr1 != removeParentheses(splitStr1))
                        {
                            splitStr1 = removeParentheses(splitStr1);
                            lengthStrOr += 2;
                        }
                        if (splitStr2 != removeParentheses(splitStr2))
                        {
                            splitStr2 = removeParentheses(splitStr2);
                            lengthStrOr += 2;
                        }

                        //Lấy phần tử được tách chuỗi trước đó 
                        if (resultSplitOr.Count != 0)
                        {
                            resultSplitOr.Pop();
                        }

                        //Thêm các phần tử mới được tách
                        resultSplitOr.Push(splitStr1);
                        resultSplitOr.Push(splitStr2);

                        tempa = splitStr2;

                    }
                }

                foreach (var item in resultSplitOr)
                {
                    if (item.Length != 1)
                    {
                        temp = "(-(" + item + "))." + temp;
                    }
                    else
                    {
                        temp = "(-" + item + ")." + temp;
                    }
                }

                if (temp != "")
                {
                    temp = temp.Substring(0, temp.Length - 1);

                    if (!result.Any(item => item == temp))
                    {
                        result.Add(temp);
                    }
                }

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


        //Suy diễn tự nhiên cho các mệnh đề
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

                //Xử lý hàm EQ ==========================================================

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

            //List<string> t = new List<string>();
            //t.Add("A+(-B)+(-D)");
            //t.Add("(E.F)>D");
            //t.Add("-A");
            //t.Add("E.F");
            //
            //string result = "-B";
            //
            //for (int i = 0; i < t.Count; i++)
            //{
            //    Console.WriteLine(i.ToString() + ". " + t[i]);
            //}
            //Console.WriteLine("------------");

            //handleClause(t, result);


            List<string> test = EQ("-(a+(b.c)+d+(e>f))");
            foreach (var item in test)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
