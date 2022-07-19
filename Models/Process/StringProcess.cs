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
    }
}