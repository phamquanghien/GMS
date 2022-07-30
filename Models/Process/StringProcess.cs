using System.Text.RegularExpressions;
using System;
namespace GSM.Models
{
    public class StringProcess
    {
        //phuong thuc voi tham so dau vao la kieu string
        //gia su sinh ma tu dong cho doi tuong Product (PRO001, PRO002...)
        //ma san pham moi nhat la PRO009 => id = "PRO009"
        public string GenerateKey (string id){
            //khai bao 1 bien de chua gia tri cua ma san pham
            string strkey = "";
            //khai bao 2 bien de chua phan so va phan chua của tham so dau vao (id = "PRO009")
            string numPart = "", strPart = "";
            //tach lay phan so cua tham so dau vao => "009"
            numPart = Regex.Match(id, @"\d+").Value;
            //tach lay phan chu cua tham so dau vao => "PRO"
            strPart = Regex.Match(id, @"\D+").Value;
            //khai bao 1 bien kieu int = phan so tang len 1 don vi
            //=> Convert.ToInt32("009") + 1 = 10
            int intPart = (Convert.ToInt32(numPart) + 1);
            //them vao cac ky tu so 0 => Do dai chuan theo quy din
            // => phan so cua ma san pham => "010"
            for (int i = 0; i < numPart.Length - intPart.ToString().Length; i++)
            {
                strPart += "0";
            }
            //ghep phan so va phan chu de tao ra ma moi (ma san pham tu sinh)
            // => "PRO" + "010" = "PRO010"
            strkey = strPart + intPart;
            //tra ve ma tu sinh ra
            return strkey;
        }
        //ket thuc phuong thuc tra ve ma san pham la: "PRO010"
        //NumberToText
        public string NumberToText(double inputNumber, bool suffix = true)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber.ToString("#");
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
            number = -number;
            sNumber = number.ToString();
            isNegative = true;
            }
            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";
            if (positionDigit == 0)
            result = unitNumbers[0] + result;
            else
            {
            // 0:       ###
            // 1: nghìn ###,###
            // 2: triệu ###,###,###
            // 3: tỷ    ###,###,###,###
            int placeValue = 0;

            while (positionDigit > 0)
            {
                // Check last 3 digits remain ### (hundreds tens ones)
                tens = hundreds = -1;
                ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                positionDigit--;
                if (positionDigit > 0)
                {
                tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                positionDigit--;
                if (positionDigit > 0)
                {
                    hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                }
                }

                if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                result = placeValues[placeValue] + result;

                placeValue++;
                if (placeValue > 3) placeValue = 1;

                if ((ones == 1) && (tens > 1))
                result = "một " + result;
                else
                {
                if ((ones == 5) && (tens > 0))
                    result = "lăm " + result;
                else if (ones > 0)
                    result = unitNumbers[ones] + " " + result;
                }
                if (tens < 0)
                break;
                else
                {
                if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                if (tens == 1) result = "mười " + result;
                if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                }
                if (hundreds < 0) break;
                else
                {
                if ((hundreds > 0) || (tens > 0) || (ones > 0))
                    result = unitNumbers[hundreds] + " trăm " + result;
                }
                result = " " + result;
            }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            return result + (suffix ? " đồng" : "");
        }
        //capitalize first letter
        public string CapitalizeFirstLetter(string strInput)
        {
            string strFirst = strInput.Substring(0,1);
            strFirst = strFirst.ToUpper();
            string strEnd = strInput.Substring(1);
            return strFirst + strEnd;
        }
    }
}