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
                int[] openParentheses = new int[100]; //Danh sách vị trí dấu mở ngoặc '('
                int[] closeParentheses = new int[100]; //Danh sách vị trí dấu đóng ngoặc ')'
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
                            if(flag < 0)
                            {
                                return false;
                            }
                        }

                        if (flag != -1)
                        {
                            closeParentheses[flag] = i;
                        }
                    }
                }

                if (closeParentheses[0] == lastIndex && firstIndex == 0 && lastIndex == a.Length-1)
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

                    if (countCharacter(a, '(') == countCharacter(a, ')') && countCharacter(b1, '(') == countCharacter(b1, ')') && countCharacter(b2, '(') == countCharacter(b2, ')') && countCharacter(c, '(') == countCharacter(c, ')'))
                    {
                        b1 = removeParentheses(b1);
                        b2 = removeParentheses(b2);
                        a = removeParentheses(a);
                        c = removeParentheses(c);
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

                        if (result != "")
                        {
                            return result;
                        }
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
        static string CON(List<string> clauses, string c1, string c2)
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
            List<string> EQResult = EQ(result);
            if (EQResult.Any(temp => temp == "0"))
            {
                return "0";
            }
            else
            {
                foreach (var item in clauses)
                {
                    if (item.Contains(result))
                    {
                        return result;
                    }
                }
            }

            return "";
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
            clause = removeParentheses(clause);
            List<string> result = new List<string>();
            string a = "", b = "";
            List<int> indexAndOfClauseList = findCharacter(clause, '.');
            List<int> indexOrOfClauseList = findCharacter(clause, '+');
            List<int> indexEqualClauseList = findCharacter(clause, '=');
            List<int> indexDeducedClauseList = findCharacter(clause, '>');

            for (int i = 0; i < indexDeducedClauseList.Count; i++)
            {
                int index = indexDeducedClauseList[i];
                //Tách a và b từ clause
                a = clause.Substring(0, index);
                b = clause.Substring(index + 1, clause.Length - index - 1);

                //Kiểm tra nếu số lượng dấu '(' = số lượng dấu ')' thì xử lý
                if (countCharacter(a, '(') == countCharacter(a, ')') && countCharacter(b, '(') == countCharacter(b, ')'))
                {
                    //4.2) a>b = (-a)+b =================================================================================================
                    string tempa = removeParentheses(a);
                    if(tempa[0] == '-')
                    {
                        tempa = tempa.Substring(1, tempa.Length - 1);
                        //tempa = removeParentheses(tempa);
                    }
                    else if(tempa.Length != 1)
                    {
                        tempa = string.Format("(-({0}))", tempa);
                    }
                    else
                    {
                        tempa = string.Format("(-{0})", tempa);
                    }

                    string temp = string.Format("{0}+{1}", tempa, b);
                    if (!result.Any(item => item == temp) && temp != "")
                    {
                        result.Add(temp);
                    }

                    //4.3.1) a>b = (-b)>(-a) =================================================================================================
                    string tempb = removeParentheses(b);
                    if (tempb[0] == '-')
                    {
                        tempb = tempb.Substring(1, tempb.Length - 1);
                        //tempb = removeParentheses(tempb);
                    }
                    else if (tempb.Length != 1)
                    {
                        tempb = string.Format("(-({0}))", tempb);
                    }
                    else
                    {
                        tempb = string.Format("(-{0})", tempb);
                    }

                    temp = string.Format("{0}>{1}", tempb, tempa);
                    if (!result.Any(item => item == temp) && temp != "")
                    {
                        result.Add(temp);
                    }

                }
            }

             for ( int i = 0; i < indexEqualClauseList.Count; i++)
             {
                int index = indexEqualClauseList[i];
                //Tách a và b từ clause
                a = clause.Substring(0, index);
                b = clause.Substring(index + 1, clause.Length - index - 1);

                //Kiểm tra nếu số lượng dấu '(' = số lượng dấu ')' thì xử lý
                if (countCharacter(a, '(') == countCharacter(a, ')') && countCharacter(b, '(') == countCharacter(b, ')'))
                {
                    //4.1) a=b = (a>b).(b.a) =================================================================================================
                    string temp = string.Format("({0}>{1}).({2}>{3})", a, b, b, a);
                    if(!result.Any(item => item == temp) && temp != "")
                    {
                        result.Add(temp);
                    }
                }
            }

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

                    // 5.2.1) (-a).(-b)= -(a+b) =================================================================================================
                    string tempRmvParenthesesA = removeParentheses(a);
                    string tempRmvParenthesesB = removeParentheses(b);
                    //Xử lý với mệnh đề a
                    if (tempRmvParenthesesA[0] == '-')
                    {
                        tempRmvParenthesesA = tempRmvParenthesesA.Substring(1, tempRmvParenthesesA.Length - 1);
                        //tempRmvParenthesesA = removeParentheses(tempRmvParenthesesA);
                    }
                    else if (tempRmvParenthesesA.Length != 1)
                    {
                        tempRmvParenthesesA = string.Format("(-({0}))",tempRmvParenthesesA);
                    }
                    else
                    {
                        tempRmvParenthesesA = string.Format("(-{0})", tempRmvParenthesesA);
                    }

                    //Xử lý với mệnh đề b
                    if (tempRmvParenthesesB[0] == '-')
                    {
                        tempRmvParenthesesB = tempRmvParenthesesB.Substring(1, tempRmvParenthesesB.Length - 1);
                        //tempRmvParenthesesB = removeParentheses(tempRmvParenthesesB);
                    }
                    else if (tempRmvParenthesesB.Length != 1)
                    {
                        tempRmvParenthesesB = string.Format("(-({0}))", tempRmvParenthesesB);
                    }
                    else
                    {
                        tempRmvParenthesesB = string.Format("(-{0})", tempRmvParenthesesB);
                    }
                    temp = string.Format("-({0}+{1})", tempRmvParenthesesA, tempRmvParenthesesB);
                    if(!result.Any(item => item == temp) && temp != "")
                    {
                        result.Add(temp);
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

                    
                    string tempRmvParenthesesA = removeParentheses(a);
                    string tempRmvParenthesesB = removeParentheses(b);
                    //Xử lý với mệnh đề a
                    if (tempRmvParenthesesA[0] == '-')
                    {
                        tempRmvParenthesesA = tempRmvParenthesesA.Substring(1, tempRmvParenthesesA.Length - 1);
                        //tempRmvParenthesesA = removeParentheses(tempRmvParenthesesA);
                    }
                    else if (tempRmvParenthesesA.Length != 1)
                    {
                        tempRmvParenthesesA = string.Format("(-({0}))", tempRmvParenthesesA);
                    }
                    else
                    {
                        tempRmvParenthesesA = string.Format("(-{0})", tempRmvParenthesesA);
                    }

                    //Xử lý với mệnh đề b
                    if (tempRmvParenthesesB[0] == '-')
                    {
                        tempRmvParenthesesB = tempRmvParenthesesB.Substring(1, tempRmvParenthesesB.Length - 1);
                        //tempRmvParenthesesB = removeParentheses(tempRmvParenthesesB);
                    }
                    else if (tempRmvParenthesesB.Length != 1)
                    {
                        tempRmvParenthesesB = string.Format("(-({0}))", tempRmvParenthesesB);
                    }
                    else
                    {
                        tempRmvParenthesesB = string.Format("(-{0})", tempRmvParenthesesB);
                    }

                    //4.2) (-a)+b = a>b =================================================================================================
                    temp = string.Format("{0}>{1}", tempRmvParenthesesA, b);
                    if (!result.Any(item => item == temp) && temp != "")
                    {
                        result.Add(temp);
                    }

                    //5.2.2) (-a)+(-b)= -(a.b) =================================================================================================
                    temp = string.Format("-({0}.{1})", tempRmvParenthesesA, tempRmvParenthesesB);
                    if (!result.Any(item => item == temp) && temp != "")
                    {
                        result.Add(temp);
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

                //5.2.2) -(a.b) = (-a)+(-b) =================================================================================================
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
                        }
                        if (splitStr2 != removeParentheses(splitStr2))
                        {
                            splitStr2 = removeParentheses(splitStr2);
                            lengthStrAnd += 1;
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
                        string tmp = "-(" + item + ")";
                        tmp = EQ5_1(tmp);
                        tmp = removeParentheses(tmp);
                        if (tmp.Length != 1)
                        {
                            temp = "(" + tmp + ")+" + temp;
                        }
                        else
                        {
                            temp = tmp + "+" + temp;
                        }
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

                //5.3.2) -(a+b) = (-a).(-b) =================================================================================================


                temp = "";
                //Loại bỏ dấu trừ và dấu ngoặc
                tempa = clause.Substring(1, clause.Length - 1);
                tempa = removeParentheses(tempa);

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
                        }
                        if (splitStr2 != removeParentheses(splitStr2))
                        {
                            splitStr2 = removeParentheses(splitStr2);
                            lengthStrOr += 1;
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

                temp = "";
                foreach (var item in resultSplitOr)
                {
                    if (item.Length != 1)
                    {
                        string tmp = "-(" + item + ")";
                        tmp = EQ5_1(tmp);
                        tmp = removeParentheses(tmp);
                        if (tmp.Length != 1)
                        {
                            temp = "(" + tmp + ")." + temp;
                        }
                        else
                        {
                            temp = tmp + "." + temp;
                        }
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

        //5.1) -(-a)=a =================================================================================================
        static string EQ5_1(string clause)
        {
            string result = "";

            if (clause.IndexOf("-(") == 0)
            {
                result = clause;
                if (clause.IndexOf("-(-") == 0)
                {
                    result = clause.Substring(3, clause.Length - 4);
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

        //Nối danh sách
        static void addList(List<string> list, List<string> addedList)
        {
            foreach (var item in addedList)
            {
                if (!list.Any(temp => temp == item))
                {
                    list.Add(item);
                }
            }
        }

        //Tìm chuỗi con
        static List<int> findIndexSubstring(string clause, string substr)
        {
            List<int> result = new List<int>();
            string temp = clause;
            int indexOfSubstr = 0;
            while(indexOfSubstr != -1)
            {
                indexOfSubstr = temp.IndexOf(substr);
                result.Add(indexOfSubstr);
                temp = temp.Substring(indexOfSubstr, temp.Length - indexOfSubstr - 1);

            }
            return result;
        }

        //Suy diễn tự nhiên cho các mệnh đề
        static bool handleClause(List<string> clause,string result, ref List<MyClause> clauses)
        {
            if (!checkInput(clause))
            {
                Console.WriteLine("Syntax Error");
                return false;
            }

            clauses = new List<MyClause>();

            for (int i = 0; i < clause.Count; i++)
            {
                MyClause temp = new MyClause();
                temp.type = "CP";
                temp.id = i;
                temp.content = clause[i];
                clauses.Add(temp);
            }

            for(int i = 0; i < clause.Count; i++)
            {
                if (clause[i] == "0")
                {
                    return false;
                }
                else if(clause[i] == "1")
                {
                    Console.WriteLine("TRUE");
                    return true;
                }

                //Xử lý hàm SIM
                string SIMResult1 = "", SIMResult2 = "";
                SIMResult1 = SIM(clause[i], 1);
                SIMResult2 = SIM(clause[i], 2);

                //Xử lý hàm EQ
                List<string> EQResult = new List<string>();
                EQResult = EQ(clause[i]);
                int m = clause.Count;
                addList(clause, EQResult);

                for (; m < clause.Count; m++)
                {
                    MyClause c = new MyClause();
                    c.content = clause[m];
                    c.type = "EQ";
                    c.num.Add(i);
                    c.id = m;
                    clauses.Add(c);
                    Console.WriteLine("{0}. {1} | EQ {2}", m, clause[m], i);
                }
                


                //Nếu có kết quả
                if (SIMResult1 != "")
                {
                    if (!clause.Any(item => item == SIMResult1)) //Nếu kết quả không tồn tại trong tập các mệnh đề
                    {
                        clause.Add(SIMResult1);

                        MyClause c = new MyClause();
                        c.type = "SIM";
                        c.num.Add(i);
                        c.content = SIMResult1;
                        c.id = clause.Count - 1;
                        clauses.Add(c);

                        Console.WriteLine("{0}. {1} | SIM {2}",clause.Count - 1,SIMResult1, i);
                        if (SIMResult1 == result)
                        {
                            return true;
                        }
                    }
                    if (!clause.Any(item => item == SIMResult2)) //Nếu kết quả không tồn tại trong tập các mệnh đề
                    {
                        clause.Add(SIMResult2);

                        MyClause c = new MyClause();
                        c.type = "SIM";
                        c.num.Add(i);
                        c.content = SIMResult2;
                        c.id = clause.Count - 1;
                        clauses.Add(c);

                        Console.WriteLine("{0}. {1} | SIM {2}", clause.Count - 1, SIMResult2,i);
                        if (SIMResult2 == result)
                        {
                            return true;
                        }
                    }
                    
                }

                for (int j = 0; j < clause.Count; j++)
                {
                    //Xử lý lệnh ADD
                    string temp = clause[j];

                    if (temp.Length != 1)
                    {
                        temp = "(" + temp + ")";
                    }
                    string temp1 = "(" + temp + "+";
                    string temp2 = "+" + temp + ")";

                    if (clause[i].IndexOf(temp1) != -1)
                    {
                        int index = clause[i].IndexOf(temp1);
                        temp = clause[i].Substring(index, temp1.Length + 2);
                    }
                    else if (clause[i].IndexOf(temp2) != -1)
                    {
                        int index = clause[i].IndexOf(temp2);
                        temp = clause[i].Substring(index - 2, temp2.Length + 2);
                    }

                    if (countCharacter(temp, '(') == countCharacter(temp, ')'))
                    {
                        temp = removeParentheses(temp);
                        if (!clause.Any(item => item == temp))
                        {
                            clause.Add(temp);

                            MyClause c = new MyClause();
                            c.type = "ADD";
                            c.num.Add(j);
                            c.content = temp;
                            c.id = clause.Count - 1;
                            clauses.Add(c);
                            
                            Console.WriteLine("{0}. {1} | ADD {2}", clause.Count - 1, temp, j);
                            
                            if (temp == result)
                            {
                                return true;
                            }
                        }
                    }


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
                            case 4: tempResult = CON(clause, clause[i], clause[j]); break;
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

                            MyClause c = new MyClause();
                            c.content = tempResult;
                            c.id = clause.Count - 1;
                            Console.Write("{0}. {1}", clause.Count -1, tempResult);
                            switch (select)
                            {
                                case 1: c.type = "MP"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | MP {0},{1}", i, j);
                                    break;
                                case 2: c.type = "MT"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | MT {0},{1}", i, j);
                                    break;
                                case 3: c.type = "DS"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | DS {0},{1}", i, j);
                                    break;
                                case 4: c.type = "CON"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | CON {0},{1}", i, j);
                                    break;
                                case 5: c.type = "HS"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | HS {0},{1}", i, j);
                                    break;
                                case 6: break;
                            }

                            clauses.Add(c);
                           
                            if (tempResult == "0")
                            {
                                exportResult(clauses);
                                return false;
                            }

                            if (tempResult == result)
                            {
                                //Console.WriteLine("TRUE");
                                return true;
                            }
                            i = 0;
                        }
                    }

                    for (int k = 0; k < clause.Count; k++)
                    {
                        string DILResult = "";
                        DILResult = DIL(clause[i], clause[j], clause[k]);
                        if (DILResult != "")
                        {
                            //
                            if (!clause.Any(item => item == DILResult))
                            {
                                clause.Add(DILResult);

                                MyClause c = new MyClause();
                                c.content = DILResult;
                                c.type = "DIL";
                                c.id = clause.Count - 1;
                                c.num.Add(i); c.num.Add(j); c.num.Add(k);
                                clauses.Add(c);
                                
                                Console.WriteLine("{0}. {1} | DIL {2},{3},{4}", clause.Count - 1, DILResult, i, j, k);
                                if (DILResult == result)
                                {
                                    return true;
                                }
                                i = 0; j = 0;
                            }

                        }

                    }
                }
            }
            return false;
        }
        static bool handleClauseCondition(List<string> clause, string result, ref List<MyClause> clauses, ref Stack<string> SplitDeducedStack)
        {
            if (!checkInput(clause))
            {
                Console.WriteLine("Syntax Error");
                return false;
            }

            List<int> iDeducedList = findCharacter(result, '>');
            string tempa = result;
            //index - Lưu vị trí được tách thành các chuỗi 
            int lengthStrDeduced = 0;
            //Mảng chuỗi kết quả
            Stack<string> resultSplitDeduced = new Stack<string>();
            for (int i = 0; i < iDeducedList.Count; i++)
            {
                //Tách 2 chuỗi tại vị trí dấu '.'
                string splitStr1 = tempa.Substring(0, iDeducedList[i] - lengthStrDeduced);
                string splitStr2 = tempa.Substring(iDeducedList[i] - lengthStrDeduced + 1, tempa.Length - iDeducedList[i] + lengthStrDeduced - 1);

                if (countCharacter(splitStr1, '(') == countCharacter(splitStr1, ')') && countCharacter(splitStr2, '(') == countCharacter(splitStr2, ')'))
                {
                    lengthStrDeduced = splitStr1.Length + 1;

                    //Xóa ngoặc đơn
                    if (splitStr1 != removeParentheses(splitStr1))
                    {
                        splitStr1 = removeParentheses(splitStr1);
                    }
                    if (splitStr2 != removeParentheses(splitStr2))
                    {
                        splitStr2 = removeParentheses(splitStr2);
                        lengthStrDeduced += 1;
                    }

                    //Lấy phần tử được tách chuỗi trước đó 
                    if (resultSplitDeduced.Count != 0)
                    {
                        resultSplitDeduced.Pop();
                        SplitDeducedStack.Pop();
                    }

                    //Thêm các phần tử mới được tách
                    resultSplitDeduced.Push(splitStr1);
                    resultSplitDeduced.Push(splitStr2);
                    SplitDeducedStack.Push(splitStr1);
                    SplitDeducedStack.Push(splitStr2);

                    tempa = splitStr2;
                }
            }

            if(resultSplitDeduced.Count >= 2)
            {
                result = resultSplitDeduced.Pop();
                while(resultSplitDeduced.Count != 0)
                {
                    clause.Add(resultSplitDeduced.Pop());
                }
            }

            clauses = new List<MyClause>();

            for (int i = 0; i < clause.Count; i++)
            {
                MyClause temp = new MyClause();
                temp.type = "CP";
                temp.id = i;
                temp.content = clause[i];
                clauses.Add(temp);
            }

            for (int i = 0; i < clause.Count; i++)
            {
                if (clause[i] == "0")
                {
                    return false;
                }
                else if (clause[i] == "1")
                {
                    Console.WriteLine("TRUE");
                    return true;
                }

                //Xử lý hàm SIM
                string SIMResult1 = "", SIMResult2 = "";
                SIMResult1 = SIM(clause[i], 1);
                SIMResult2 = SIM(clause[i], 2);

                //Xử lý hàm EQ
                List<string> EQResult = new List<string>();
                EQResult = EQ(clause[i]);
                int m = clause.Count;
                addList(clause, EQResult);

                for (; m < clause.Count; m++)
                {
                    MyClause c = new MyClause();
                    c.content = clause[m];
                    c.type = "EQ";
                    c.num.Add(i);
                    c.id = m;
                    clauses.Add(c);
                    Console.WriteLine("{0}. {1} | EQ {2}", m, clause[m], i);
                }



                //Nếu có kết quả
                if (SIMResult1 != "")
                {
                    if (!clause.Any(item => item == SIMResult1)) //Nếu kết quả không tồn tại trong tập các mệnh đề
                    {
                        clause.Add(SIMResult1);

                        MyClause c = new MyClause();
                        c.type = "SIM";
                        c.num.Add(i);
                        c.content = SIMResult1;
                        c.id = clause.Count - 1;
                        clauses.Add(c);

                        Console.WriteLine("{0}. {1} | SIM {2}", clause.Count - 1, SIMResult1, i);
                        if (SIMResult1 == result)
                        {
                            return true;
                        }
                    }
                    if (!clause.Any(item => item == SIMResult2)) //Nếu kết quả không tồn tại trong tập các mệnh đề
                    {
                        clause.Add(SIMResult2);

                        MyClause c = new MyClause();
                        c.type = "SIM";
                        c.num.Add(i);
                        c.content = SIMResult2;
                        c.id = clause.Count - 1;
                        clauses.Add(c);

                        Console.WriteLine("{0}. {1} | SIM {2}", clause.Count - 1, SIMResult2, i);
                        if (SIMResult2 == result)
                        {
                            return true;
                        }
                    }

                }

                for (int j = 0; j < clause.Count; j++)
                {
                    //Xử lý lệnh ADD
                    string temp = clause[j];

                    if (temp.Length != 1)
                    {
                        temp = "(" + temp + ")";
                    }
                    string temp1 = "(" + temp + "+";
                    string temp2 = "+" + temp + ")";

                    if (clause[i].IndexOf(temp1) != -1)
                    {
                        int index = clause[i].IndexOf(temp1);
                        temp = clause[i].Substring(index, temp1.Length + 2);
                    }
                    else if (clause[i].IndexOf(temp2) != -1)
                    {
                        int index = clause[i].IndexOf(temp2);
                        temp = clause[i].Substring(index - 2, temp2.Length + 2);
                    }

                    if (countCharacter(temp, '(') == countCharacter(temp, ')'))
                    {
                        temp = removeParentheses(temp);
                        if (!clause.Any(item => item == temp))
                        {
                            clause.Add(temp);

                            MyClause c = new MyClause();
                            c.type = "ADD";
                            c.num.Add(j);
                            c.content = temp;
                            c.id = clause.Count - 1;
                            clauses.Add(c);

                            Console.WriteLine("{0}. {1} | ADD {2}", clause.Count - 1, temp, j);

                            if (temp == result)
                            {
                                return true;
                            }
                        }
                    }


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
                            case 4: tempResult = CON(clause, clause[i], clause[j]); break;
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

                            MyClause c = new MyClause();
                            c.content = tempResult;
                            c.id = clause.Count - 1;
                            Console.Write("{0}. {1}", clause.Count - 1, tempResult);
                            switch (select)
                            {
                                case 1:
                                    c.type = "MP"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | MP {0},{1}", i, j);
                                    break;
                                case 2:
                                    c.type = "MT"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | MT {0},{1}", i, j);
                                    break;
                                case 3:
                                    c.type = "DS"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | DS {0},{1}", i, j);
                                    break;
                                case 4:
                                    c.type = "CON"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | CON {0},{1}", i, j);
                                    break;
                                case 5:
                                    c.type = "HS"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | HS {0},{1}", i, j);
                                    break;
                                case 6: break;
                            }

                            clauses.Add(c);

                            if (tempResult == "0")
                            {
                                exportResult(clauses);
                                return false;
                            }

                            if (tempResult == result)
                            {
                                //Console.WriteLine("TRUE");
                                return true;
                            }
                            i = 0;
                        }
                    }

                    for (int k = 0; k < clause.Count; k++)
                    {
                        string DILResult = "";
                        DILResult = DIL(clause[i], clause[j], clause[k]);
                        if (DILResult != "")
                        {
                            //
                            if (!clause.Any(item => item == DILResult))
                            {
                                clause.Add(DILResult);

                                MyClause c = new MyClause();
                                c.content = DILResult;
                                c.type = "DIL";
                                c.id = clause.Count - 1;
                                c.num.Add(i); c.num.Add(j); c.num.Add(k);
                                clauses.Add(c);

                                Console.WriteLine("{0}. {1} | DIL {2},{3},{4}", clause.Count - 1, DILResult, i, j, k);
                                if (DILResult == result)
                                {
                                    return true;
                                }
                                i = 0; j = 0;
                            }

                        }

                    }
                }
            }
            return false;
        }
        static bool handleClauseCounter(List<string> clause, string result, ref List<MyClause> clauses)
        {
            if (!checkInput(clause))
            {
                Console.WriteLine("Syntax Error");
                return false;
            }

            if(result[0] == '-')
            {
                result = result.Substring(1, result.Length - 1);
                result = removeParentheses(result);
            }
            else
            {
                if (result.Length == 1)
                {
                    result = "-" + result;
                }
                else
                {
                    result = "-(" + result + ")";
                }
            }
            clause.Add(result);
            result = "0";

            clauses = new List<MyClause>();

            for (int i = 0; i < clause.Count; i++)
            {
                MyClause temp = new MyClause();
                temp.type = "CP";
                temp.id = i;
                temp.content = clause[i];
                clauses.Add(temp);
            }

            for (int i = 0; i < clause.Count; i++)
            {
                if (clause[i] == "0")
                {
                    return false;
                }
                else if (clause[i] == "1")
                {
                    Console.WriteLine("TRUE");
                    return true;
                }

                //Xử lý hàm SIM
                string SIMResult1 = "", SIMResult2 = "";
                SIMResult1 = SIM(clause[i], 1);
                SIMResult2 = SIM(clause[i], 2);

                //Xử lý hàm EQ
                List<string> EQResult = new List<string>();
                EQResult = EQ(clause[i]);
                int m = clause.Count;
                addList(clause, EQResult);

                for (; m < clause.Count; m++)
                {
                    MyClause c = new MyClause();
                    c.content = clause[m];
                    c.type = "EQ";
                    c.num.Add(i);
                    c.id = m;
                    clauses.Add(c);
                    Console.WriteLine("{0}. {1} | EQ {2}", m, clause[m], i);
                }



                //Nếu có kết quả
                if (SIMResult1 != "")
                {
                    if (!clause.Any(item => item == SIMResult1)) //Nếu kết quả không tồn tại trong tập các mệnh đề
                    {
                        clause.Add(SIMResult1);

                        MyClause c = new MyClause();
                        c.type = "SIM";
                        c.num.Add(i);
                        c.content = SIMResult1;
                        c.id = clause.Count - 1;
                        clauses.Add(c);

                        Console.WriteLine("{0}. {1} | SIM {2}", clause.Count - 1, SIMResult1, i);
                        if (SIMResult1 == result)
                        {
                            return true;
                        }
                    }
                    if (!clause.Any(item => item == SIMResult2)) //Nếu kết quả không tồn tại trong tập các mệnh đề
                    {
                        clause.Add(SIMResult2);

                        MyClause c = new MyClause();
                        c.type = "SIM";
                        c.num.Add(i);
                        c.content = SIMResult2;
                        c.id = clause.Count - 1;
                        clauses.Add(c);

                        Console.WriteLine("{0}. {1} | SIM {2}", clause.Count - 1, SIMResult2, i);
                        if (SIMResult2 == result)
                        {
                            return true;
                        }
                    }

                }

                for (int j = 0; j < clause.Count; j++)
                {
                    //Xử lý lệnh ADD
                    string temp = clause[j];

                    if (temp.Length != 1)
                    {
                        temp = "(" + temp + ")";
                    }
                    string temp1 = "(" + temp + "+";
                    string temp2 = "+" + temp + ")";

                    if (clause[i].IndexOf(temp1) != -1)
                    {
                        int index = clause[i].IndexOf(temp1);
                        temp = clause[i].Substring(index, temp1.Length + 2);
                    }
                    else if (clause[i].IndexOf(temp2) != -1)
                    {
                        int index = clause[i].IndexOf(temp2);
                        temp = clause[i].Substring(index - 2, temp2.Length + 2);
                    }

                    if (countCharacter(temp, '(') == countCharacter(temp, ')'))
                    {
                        temp = removeParentheses(temp);
                        if (!clause.Any(item => item == temp))
                        {
                            clause.Add(temp);

                            MyClause c = new MyClause();
                            c.type = "ADD";
                            c.num.Add(j);
                            c.content = temp;
                            c.id = clause.Count - 1;
                            clauses.Add(c);

                            Console.WriteLine("{0}. {1} | ADD {2}", clause.Count - 1, temp, j);

                            if (temp == result)
                            {
                                return true;
                            }
                        }
                    }


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
                            case 4: tempResult = CON(clause, clause[i], clause[j]); break;
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

                            MyClause c = new MyClause();
                            c.content = tempResult;
                            c.id = clause.Count - 1;
                            Console.Write("{0}. {1}", clause.Count - 1, tempResult);
                            switch (select)
                            {
                                case 1:
                                    c.type = "MP"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | MP {0},{1}", i, j);
                                    break;
                                case 2:
                                    c.type = "MT"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | MT {0},{1}", i, j);
                                    break;
                                case 3:
                                    c.type = "DS"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | DS {0},{1}", i, j);
                                    break;
                                case 4:
                                    c.type = "CON"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | CON {0},{1}", i, j);
                                    break;
                                case 5:
                                    c.type = "HS"; c.num.Add(i); c.num.Add(j); Console.WriteLine(" | HS {0},{1}", i, j);
                                    break;
                                case 6: break;
                            }

                            clauses.Add(c);

                            if (tempResult == "0")
                            {
                                exportResult(clauses);
                                return false;
                            }

                            if (tempResult == result)
                            {
                                //Console.WriteLine("TRUE");
                                return true;
                            }
                            i = 0;
                        }
                    }

                    for (int k = 0; k < clause.Count; k++)
                    {
                        string DILResult = "";
                        DILResult = DIL(clause[i], clause[j], clause[k]);
                        if (DILResult != "")
                        {
                            //
                            if (!clause.Any(item => item == DILResult))
                            {
                                clause.Add(DILResult);

                                MyClause c = new MyClause();
                                c.content = DILResult;
                                c.type = "DIL";
                                c.id = clause.Count - 1;
                                c.num.Add(i); c.num.Add(j); c.num.Add(k);
                                clauses.Add(c);

                                Console.WriteLine("{0}. {1} | DIL {2},{3},{4}", clause.Count - 1, DILResult, i, j, k);
                                if (DILResult == result)
                                {
                                    return true;
                                }
                                i = 0; j = 0;
                            }

                        }

                    }
                }
            }
            return false;
        }

        //Xuất kết quả 
        static public void exportResult(List<MyClause> clauses)
        {
            List<MyClause> steps = new List<MyClause>();
            steps.Add(clauses[clauses.Count - 1]);
            int newCount = -1;
            int i = 0;

            //Thêm các phần tử có liên quan tới mệnh đề kết quả vào mảng "steps"
            while (i != newCount)
            {
                foreach(int item in steps[i].num)
                {
                    if(!steps.Any(temp => temp == clauses[item]))
                    {
                        steps.Add(clauses[item]);
                    }
                }
                i++;
                newCount = steps.Count;
            }

            //Sắp xếp lại các phần tử
            List<MyClause> SortedList = steps.OrderBy(o => o.id).ToList();

            //Chỉnh sửa thuộc tính 'num' trong mỗi phần tử
            for (int index = 0; index < SortedList.Count; index++)
            {
                for (int k = 0; k < SortedList[index].num.Count; k++)
                {
                    for (int j = 0; j < SortedList.Count; j++)
                    {
                        if (SortedList[index].num[k] == SortedList[j].id)
                        {
                            SortedList[index].num[k] = j;
                            break;
                        }
                    }
                }
            }

            //Xuất ra màn hình kết quả cuối cùng
            Console.WriteLine("\n==========SOLUTION==========");
            Console.WriteLine("{0,-7} {1,-40} {2,-7}", "Serial", "    Clause", "Reason");

            for (int index = 0;index < SortedList.Count; index++)
            {
                Console.Write("{0,-7}|{1,-40}|{2,-7}|", index, SortedList[index].content, SortedList[index].type);
                
                foreach(var item in SortedList[index].num)
                {
                    Console.Write(" {0,-3}",item);
                }
                Console.WriteLine();
            }
        }
        static public void exportResultCondition(List<MyClause> clauses,string result, Stack<string> resultSplit)
        {
            if(resultSplit.Count >= 2)
            {
                MyClause temp = new MyClause();
                temp.content = result;
                temp.id = clauses.Count;
                foreach(string item in resultSplit)
                {
                    for(int pos = clauses.Count-1;pos >=0; pos--)
                    {
                        if(item == clauses[pos].content)
                        {
                            temp.num.Add(pos);
                            break;
                        }
                    }
                }
                clauses.Add(temp);
            }

            List<MyClause> steps = new List<MyClause>();
            steps.Add(clauses[clauses.Count - 1]);
            int newCount = -1;
            int i = 0;

            //Thêm các phần tử có liên quan tới mệnh đề kết quả vào mảng "steps"
            while (i != newCount)
            {
                foreach (int item in steps[i].num)
                {
                    if (!steps.Any(temp => temp == clauses[item]))
                    {
                        steps.Add(clauses[item]);
                    }
                }
                i++;
                newCount = steps.Count;
            }

            //Sắp xếp lại các phần tử
            List<MyClause> SortedList = steps.OrderBy(o => o.id).ToList();

            //Chỉnh sửa thuộc tính 'num' trong mỗi phần tử
            for (int index = 0; index < SortedList.Count; index++)
            {
                for (int k = 0; k < SortedList[index].num.Count; k++)
                {
                    for (int j = 0; j < SortedList.Count; j++)
                    {
                        if (SortedList[index].num[k] == SortedList[j].id)
                        {
                            SortedList[index].num[k] = j;
                            break;
                        }
                    }
                }
            }

            //Xuất ra màn hình kết quả cuối cùng
            Console.WriteLine("\n==========SOLUTION==========");
            Console.WriteLine("{0,-7} {1,-40} {2,-7}", "Serial", "    Clause", "Reason");

            for (int index = 0; index < SortedList.Count; index++)
            {
                Console.Write("{0,-7}|{1,-40}|{2,-7}|", index, SortedList[index].content, SortedList[index].type);

                foreach (var item in SortedList[index].num)
                {
                    Console.Write(" {0,-3}", item);
                }
                Console.WriteLine();
            }
        }

        public class MyClause
        {
            public string type { get; set; }
            public List<int> num { get; set; }
            public string content { get; set; }
            public int id { get; set; }

            public MyClause()
            {
                num = new List<int>();
            }
        }

        static void Main(string[] args)
        {
            int choose;
            List<string> t = new List<string>();
   
                Console.WriteLine("Input clause (:");


                t.Add("(A+B)>X");
                //t.Add("A");
                //t.Add("(-X).(-Y)");
                //t.Add("F");
                //t.Add("H");

                string result = "A>X";

                for (int i = 0; i < t.Count; i++)
                {
                    Console.WriteLine(i.ToString() + ". " + t[i]);
                }
                Console.WriteLine("------------");
                Console.WriteLine(result);




                Console.WriteLine("Processing ...\n");

                List<MyClause> c = new List<MyClause>();
                Stack<string> s = new Stack<string>();
                if (handleClauseCounter(t,result,ref c))
                {
                    exportResultCondition(c, result, s);
                }
                else
                {
                    Console.WriteLine("FALSE");
                }
            

            Console.ReadKey();
        }
    }
}
