<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Text</Namespace>
</Query>

void Main()
{
	string rand = KeyGenerator.GetUniqueKey(20);
	Console.WriteLine(rand);
}

public class KeyGenerator
{
   public static string GetUniqueKey(int maxSize)
   {
       char[] chars = new char[62];
       chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
       byte[] data = new byte[1];
       using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
       {
           crypto.GetNonZeroBytes(data);
           data = new byte[maxSize];
           crypto.GetNonZeroBytes(data);
       }
       StringBuilder result = new StringBuilder(maxSize);
       foreach (byte b in data)
       {
           result.Append(chars[b % (chars.Length)]);
       }
       return result.ToString();
   }
}

