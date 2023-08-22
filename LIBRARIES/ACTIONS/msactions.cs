using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;

namespace ACTIONS.all
{
    public partial class msactions : Imsactions
    {
        public static EncryptDecrypt _EncryptDecrypt = new EncryptDecrypt();
        public string GetClientIpAddress(HttpContext httpContext)
        {
            System.Net.IPHostEntry hostInfo;
            //Attempt to resolve the DNS connection for the given host or address (localhost)
            hostInfo = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            Console.WriteLine($"\t Canonical Name: {hostInfo.HostName}");

            //Display list of IP Addresses for this host
            Console.Write("\t IP Addresses: ");
            List<string> l = new List<string>();
            foreach (var ip in hostInfo.AddressList)
            {
                l = GetIPv4FromIpAddressList(ip.ToString(), l);
            }
            Console.WriteLine(ACTIONS.all.msactions._ToString(l));

            if (l.Count > 1)
                return l[1];
            else
                return l[0];
        }

        private static List<string> GetIPv4FromIpAddressList(string o, List<string> l)
        {
            string input = o;
            // List<string> l = new List<string>();
            System.Net.IPAddress address;
            if (System.Net.IPAddress.TryParse(input, out address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        // we have IPv4
                        Console.WriteLine("we have IPv4");
                        if (!l.Contains(o))
                        {
                            l.Add(o);
                        }

                        break;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        // we have IPv6
                        Console.WriteLine("we have IPv6");
                        break;
                    default:
                        // umm... yeah... I'm going to need to take your red packet and...
                        break;
                }
                return l;
            }
            else
            {
                return l;
            }
        }

        public T ConvertJsonObj<T>(string? jsonObj)
        {
            T request = JsonConvert.DeserializeObject<T>(jsonObj ?? string.Empty);

            return request;
        }

        public static string _ToString(Object? o)
        {
            if (o != null)
            {
                return JsonConvert.SerializeObject(o);
            }
            else
            {
                return "NULL OBJ";
            }
        }

        public static string _break = "\n\t\t ==============================\n";
        public string _break_ = _break;

        public string break_() => _break_;

        private const string Key =
            @"PABSAFMAQQBLAGUAeQBWAGEAbAB1AGUAPgA8AE0AbwBkAHUAbAB1AHMAPgBxAGcAMwA3AFYAUQA1AE8AdQBuADQAMwBDAEIAbQBkAEMATQB4AFMASQB6ADkAYQA2AHoAdgBMAEgAMAA5AGcAQgBvAG0AQwA1AEwARgBTAFkASABJAEcAZgAvAEUAZQBTAE4ASgBWAHUALwBBAEwAegBxAEgAYwA2AGIAcwBrAFgAbwBRAEIAZgBPAEUAMQBEAGMAVABKAEkATgBZAHAAWAByAFMALwBmAG4AcQBtAC8AUAB0AGQAcgBzAFcAeAB2AGEAWABwAHMARABTAG0AZwBhAEwASgBTAEEAZgBHAEkAYQA2ADUAdABCAHkAQgB6AE8AOAB0AFcAdABYAC8AaABQAG4ATABZAG0AdwBXAGoASgBFADQAQgB5AG0AUAArAC8AdABZAFIAdAB3AHMAbQBYAG4AdQBtAEkAbgA3ADQATQBTADMANAB6AGYAdgByAGcAcwA9ADwALwBNAG8AZAB1AGwAdQBzAD4APABFAHgAcABvAG4AZQBuAHQAPgBBAFEAQQBCADwALwBFAHgAcABvAG4AZQBuAHQAPgA8AFAAPgAyAEkAZQBBAGIATwBRAEkASQBzAG0AQwBTAGEAYgBCAG8AdwB5AHQAUwB0AFMAWQBZAHkAKwBLAG0AMQBYAFcASAB6AGUAWAA3AC8AMgBCADEAbgBUAGoAaABvAFYAZwA4AHMARgBxAEcAdgB1AHIATQBTAGcAZwB0AGUAegBqAEkAZgB3AGkAYQAyAGUAMgBiAEUAeQBSAHkASgBSAG4AegB2AFYAcgA4AFEAPQA9ADwALwBQAD4APABRAD4AeQBRADIAMABjAEEAbQBrAG8AVgBrAHQANgBXAHgARABJADgAYQB2AGcAMgByAFEARgB1AEQAZgBmAGwAagBZAEgARwBhAE4AUABEAE4AdABKAG0AOABaAEYAbgBpAEEASABWAEwAagAvAHoAagBQADEAWAB3AFQAMABTAGsAagB2AHQAQgBQAGoASwBpAHEAVQBDAFkAbQBpAEkAOAA3AEEAeABBAGwAdQB3AD0APQA8AC8AUQA+ADwARABQAD4ATwAvAEYAdgBOAFQAWAAvAHAAcABuAEEAagB1AEUAeQBWAEIAQQByAFgAVAA3AHoAbgBPAG4ASgBaAG0AMQBoADUASwB5AEEAVABIAGsAUwAyADYAcgBxAFgAaABCAEkAbwBZAHUAMwA4AHgAWgBlADgAegBIAFgAdABHAFcATABEADUAcAA4AGMATgAxADYAWABBAHIAcQBoAE8AdgBJAHYAVAB1AG0ARQBRAD0APQA8AC8ARABQAD4APABEAFEAPgBWAEUAVABmAFIAVABwADEAZQAzADkAUwBoAEEAegB4AGsAegBRADYANgBuADEAQgBuAE8AVgBDAEoAOABYADcAUgB1AFEAZwAvAEkATwBkAGsAMABkAHIAbgA0AFMAQQBSAGsAbwB3ADgAQQArAFMANQBTAHMAdABiAHoAUwBzAEcAOQBWAGEARQBsADIANwBqAFAANgBBAGwAaQBwAGEAbABLADAAVwA4AHcAPQA9ADwALwBEAFEAPgA8AEkAbgB2AGUAcgBzAGUAUQA+AEsAbwBuADUAdABZAGEAdABiAEwAbwBuAFYAUQBIAHQARQAwAE0AaABCAGoASgB6ADMAUwBKAEMAegBqAEUAOQBYAHUATAB2AEcAOABaADMAcABNADgANgBGAFoAQwBxAEQALwBpAFgAbABZAEMAUgBOAEYAWQBDAGwASABNAEEAZwArAE4AbABBAGEAQwBUAGcAQwBmAG0ARAAzAEIANQA1AGEAawBHAFgAUQA9AD0APAAvAEkAbgB2AGUAcgBzAGUAUQA+ADwARAA+AFoATABsADMAWAByAC8AawB2AGUAMgA0AFoAdQBIAFUAOAA3AHMAaQBBADYASwBwAEYAYQBBAEwAQgBmAGEAYgA2AEEATgBYAE4AbQBJAFoAYQB1AHIAZgBFAHIAVQBjAHYAUQBGAG8AcQByAEwAYQBLADQAQQBRAE8ANQBrAFAAUgA3AFIAawB0AFQAVQBuAG0AWQBvAHYAbgAzAFYANgBkADUAQQBUAHcANwAxAEgASABRAE4ANgBVAEoAWABmAHgANQBWADMAdABYADYATgBpADgAawBnAEMAUAB3AEUANABFAEkAMQBqAHoAdgBQAFgARABUAFEARQBvAGsAOABGADUANwB3AFUAbQBqAFAASQAzAFMAVwA4ADEAQgBUADIANABYAHYASAA3AHEAagBEADQATwBLAHYAZgA3AE4AKwBOAG4ATQB2AEEAQQBoAGwATQBFAD0APAAvAEQAPgA8AC8AUgBTAEEASwBlAHkAVgBhAGwAdQBlAD4A";
        private static string PRIVATE_ENCRYPTKEY
        {
            get => "SENDESDHAITI|";
        }
        private static string PUBLIC_ENCRYPTKEY
        {
            get => "MINTSOUPSERVICES";
        }

        //public ConvType<T> ConvType;
        public ConvType<object> GetConvType<T>(Type t, object o)
        {
            return new ConvType<object>(t.GetType(), o);
        }

        public ConvType<object, object> GetConvType<T, C>(Type t, object o)
        {
            return new ConvType<object, object>(t.GetType(), o);
        }

        //public

        public class BinarySearch<Key, Value> where Key : IComparable<Key>
        {
            public Node<Key, Value> root;

            public BinarySearch(Key key, Value value)
            {
                root = new Node<Key, Value>(key, value);
                root.NotNull = true;
                Console.WriteLine("Root Node Created");
            }

            public void AddNode(Key k, Value item)
            {
                //this.root.
                //Node<Key, Value> newNode = new Node<Key, Value>();
                //newNode.element = item;
                if (root == null)
                {
                    root = new Node<Key, Value>(k, item);
                }
                root.Insert(k, item, root);
            }

            public class Node<K, V> where K : IComparable<K>
            {
                public bool NotNull;
                public K key;
                public V element;
                public Node<K, V>? left;
                public Node<K, V>? right;
                public int height;

                public Node(K _key, V data, Node<K, V>? _left, Node<K, V>? _right)
                {
                    Console.WriteLine(_break + $"Node Created with {_key}" + _break);
                    NotNull = true;
                    key = _key;
                    element = data;
                    left = _left;
                    right = _right;
                    height = 0;
                }

                public Node(K _key, V data)
                {
                    Console.WriteLine(_break + $"Node Created with {_key}" + _break);
                    NotNull = true;
                    key = _key;
                    element = data;
                    left = null;
                    right = null;

                    height = 0;
                }

                public List<V> SearchNodes(String element, Node<K, V>? root, List<V> _return)
                {
                    Console.WriteLine("Searching Node for " + element);
                    try
                    {
                        Node<K, V>? current = root;

                        if (current == null)
                        {
                            Console.WriteLine("No Node Loaded");
                            return _return;
                        }
                        else
                        {
                            foreach (String s in element.Split(" "))
                            {
                                if (s == " ")
                                {
                                    continue;
                                }
                                else
                                {
                                    if (current != null && current.key != null)
                                    {
                                        foreach (
                                            String k in (current.key.ToString() ?? "").Split(" ")
                                        )
                                        {
                                            var _k = k.Trim().ToLower();
                                            var _s = s.Trim().ToLower();
                                            if (_k.Equals(_s))
                                            {
                                                if (!_return.Contains(current.element))
                                                {
                                                    if (_k != "")
                                                    {
                                                        Console.WriteLine(
                                                            "Found " + k + " as --> " + s + _break
                                                        );
                                                        _return.Add(current.element);
                                                    }
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }

                            // if(_return)

                            if (element.CompareTo(current.key) < 0)

                                return this.SearchNodes(element, current.left, _return);
                            else

                                return this.SearchNodes(element, current.right, _return);
                        }
                    }
                    catch (Exception m)
                    {
                        Console.WriteLine(_break + m + _break);
                    }
                    finally { }
                    return _return;
                }

                public override string ToString()
                {
                    return JsonConvert.SerializeObject(this);
                }

                public string ToString(Object? o)
                {
                    if (o != null)
                    {
                        return JsonConvert.SerializeObject(o);
                    }
                    else
                    {
                        return "NULL OBJ";
                    }
                }

                /// <summary>
                /// Displays the node of the root node in order
                /// </summary>
                /// <param name="rootNode"></param>
                public void InOrder(Node<K, V>? rootNode)
                {
                    if (!(rootNode == null))
                    {
                        InOrder(rootNode.left);
                        // rootNode.DisplayNode();
                        InOrder(rootNode.right);
                    }
                }

                public void DisplayNode()
                {
                    var l = left?.GetHeight();
                    var r = right?.GetHeight();
                    Console.WriteLine(_break + "Displaying Node");
                    Console.WriteLine(
                        ToString(element) + $" {l ?? 0}:left and {r ?? 0}:right " + _break
                    );
                }

                public int CompareTo(K obj)
                {
                    if (obj.GetType() != key.GetType())
                    {
                        return -1;
                    }
                    else
                    {
                        return CompareTo(obj);
                    }
                }

                public int GetHeight()
                {
                    if (this == null)
                    {
                        return -1;
                    }
                    else
                    {
                        Console.WriteLine(
                            $"{_break} Node Height is : {this.height} for {_break} {ToString(this)}"
                        );
                        return this.height;
                    }
                }

                public Node<K, V>? Insert(K key, V item, Node<K, V>? n)
                {
                    if (n == null)
                    {
                        n = new Node<K, V>(key, item);
                    }
                    else if (key.CompareTo(n.key) < 0)
                    {
                        n.left = Insert(key, item, n.left);
                        if (n.right != null && n.left != null)
                        {
                            if ((n.left.height) - (n.right.height) == 2)
                            {
                                n = RotateWithLeftChild(n);
                            }
                            else
                            {
                                n = DoubleWithLeftChild(n);
                            }
                        }
                    }
                    else if (key.CompareTo(n.key) > 0)
                    {
                        n.right = Insert(key, item, n.right);
                        if (n.right != null && n.left != null)
                        {
                            if ((n.right.GetHeight()) - (n.left.GetHeight()) == 2)
                            {
                                if (key.CompareTo(n.right.key) > 0)
                                {
                                    n = RotateWithRightChild(n);
                                }
                                else
                                {
                                    n = DoubleWithRightChild(n);
                                }
                            }
                        }
                        else
                        {
                            //Do Nothing since it is a duplicate value
                        }
                    }

                    if (n != null)
                    {
                        if (n.left != null && n.right != null)
                        {
                            n.height = Math.Max(n.left.GetHeight(), n.right.GetHeight()) + 1;
                        }
                    }
                    return n;
                }

                private Node<K, V>? RotateWithLeftChild(Node<K, V>? n2)
                {
                    Node<K, V>? n1;
                    if (n2 != null)
                    {
                        n1 = n2.left;
                        if (n1 != null)
                        {
                            n2.left = n1.right;
                            n1.right = n2;
                            if (n2.left != null && n2.right != null)
                            {
                                n2.height = Math.Max(n2.left.GetHeight(), n2.right.GetHeight()) + 1;
                            }

                            if (n1.left != null)
                            {
                                n1.height = Math.Max(n1.left.GetHeight(), n2.GetHeight()) + 1;
                            }
                        }
                        else
                        {
                            n2.left = null;
                        }
                    }
                    else
                    {
                        n1 = null;
                    }

                    return n1;
                }

                private Node<K, V>? RotateWithRightChild(Node<K, V>? n1)
                {
                    Node<K, V>? n2;
                    if (n1 != null)
                    {
                        //if n1 is not null continue with rotating nodes
                        n2 = n1.left; //set n2 to left node of n1

                        if (n2 != null)
                        {
                            //if n2 is not null (n1.left is not null) then continue
                            if (n2.right != null)
                            {
                                //if n2's right node is not null
                                n1.left = n2.right;
                            }
                            else
                            {
                                n1.left = null;
                            }

                            if (n2.right != null)
                            {
                                //if n2's right node is not null then set
                                n2.right = n1;
                            }
                            else
                            {
                                n2.right = null;
                            }
                            if (n1.left != null && n2.right != null)
                            {
                                n1.height = Math.Max(n1.left.GetHeight(), n2.right.GetHeight()) + 1;
                                n2.height = Math.Max(n2.right.GetHeight(), n1.GetHeight()) + 1;
                            }
                        }
                    }
                    else
                    {
                        //if n1 is null set n2 to null and return
                        n2 = null;
                    }
                    return n2;
                }

                private Node<K, V>? DoubleWithLeftChild(Node<K, V>? n3)
                {
                    if (n3 != null)
                    {
                        n3.left = RotateWithRightChild(n3.left);
                    }
                    return RotateWithLeftChild(n3);
                }

                private Node<K, V>? DoubleWithRightChild(Node<K, V>? n1)
                {
                    if (n1 != null)
                    {
                        n1.right = RotateWithLeftChild(n1.right);
                    }
                    return RotateWithRightChild(n1);
                }
            }
        }

        private static string DecryptStringFromBytes(byte[]? cipherText, byte[]? key, byte[]? iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                return "NULL";
                //throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                return "NULL";
                //throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                return "NULL";
                //throw new ArgumentNullException("key");
            }
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = "";
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (
                            var csDecrypt = new CryptoStream(
                                msDecrypt,
                                decryptor,
                                CryptoStreamMode.Read
                            )
                        )
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }
            return plaintext;
        }

        public bool IsBase64String(string? base64)
        {
            if(base64 == null) {
                return false;
            }
            base64 = base64.Trim();
            return (base64.Length % 4 == 0)
                && System.Text.RegularExpressions.Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,3}$", System.Text.RegularExpressions.RegexOptions.None);
        }

        public string DecryptFromClient(string? text)
        {
            if (!IsBase64String(text)) {
                return "not encoded string spotted";
            }
            var keybytes = Encoding.UTF8.GetBytes(PUBLIC_ENCRYPTKEY);
            var iv = Encoding.UTF8.GetBytes(PUBLIC_ENCRYPTKEY);

            var encrypted = Convert.FromBase64String(text);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            Logging.Log("Decrypting", "email, id, or content", text, decriptedFromJavascript);
            return string.Format(decriptedFromJavascript);
        }

        /// <summary>
        /// To validate if a string is in email format
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true or false if the string is an email or not</returns>
        public bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public string[] GetKeyWords(string text)
        {
            var keywords = text.Split(" ");
            return keywords;
        }

        public class ConvType<T, C>
            where T : class
            where C : class
        {
            public T type;
            public object obj1;

            public ConvType(T t, object o)
            {
                type = t;
                obj1 = o;
            }

            public async Task<Dictionary<String, object>> GetTypeObjs<Ty, Class>()
            {
                await new msactions().voidTask(1);
                Dictionary<String, object> o = new Dictionary<String, object>();
                PropertyInfo[] propertyInfos;
                propertyInfos = typeof(Class).GetProperties(
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
                );
                // sort properties by name
                Array.Sort(
                    propertyInfos,
                    delegate(PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
                    {
                        return propertyInfo1.Name.CompareTo(propertyInfo2.Name);
                    }
                );

                // write property names
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    Console.WriteLine(
                        propertyInfo.Name + " " + propertyInfo.GetValue(propertyInfo.Name)
                    );

                    foreach (var method in type.GetType().GetMethods())
                    {
                        var parameters = method.GetParameters();
                        var parameterDescriptions = string.Join(
                            ", ",
                            method
                                .GetParameters()
                                .Select(x => x.ParameterType + " " + x.Name)
                                .ToArray()
                        );
                        if (
                            method.Name.Equals("Get" + propertyInfo.Name)
                            || method.Name.Contains("Get" + propertyInfo.Name)
                        )
                            Console.WriteLine(
                                "{0} {1} ({2})",
                                method.ReturnType,
                                method.Name,
                                parameterDescriptions
                            );
                        if (method.IsPublic)
                        {
                            if (method.GetParameters().Count() > 0)
                            {
                                continue;
                            }
                            else
                            {
                                o.Add(
                                    propertyInfo.Name,
                                    method.Invoke(obj1, new object[] { }) ?? new object { }
                                );
                                break;
                            }
                        }
                    }
                }
                return o;
            }
        }

        public class ConvType<T> where T : class
        {
            public T type;
            public object obj1;

            public ConvType(T t, object o)
            {
                type = t;
                obj1 = o;
            }

            public async Task<object[]> GetTypeObjs<Ty>()
            {
                await new msactions().voidTask(1);
                object[] o = new object[] { };
                foreach (var method in type.GetType().GetMethods())
                {
                    var parameters = method.GetParameters();
                    var parameterDescriptions = string.Join(
                        ", ",
                        method.GetParameters().Select(x => x.ParameterType + " " + x.Name).ToArray()
                    );

                    Console.WriteLine(
                        "{0} {1} ({2})",
                        method.ReturnType,
                        method.Name,
                        parameterDescriptions
                    );
                    if (method.IsPublic)
                    {
                        if (method.GetParameters().Count() > 0)
                        {
                            continue;
                        }
                        else
                        {
                            o.Append(method.Invoke(obj1, new object[] { }));
                        }
                    }
                }
                return o;
            }
        }

        public Microsoft.AspNetCore.Http.IFormFile? GetImage(string oldfilePath, string root)
        {
            Microsoft.AspNetCore.Http.IFormFile? file = null;
            try
            {
                Console.WriteLine(oldfilePath);
                var r = File.Exists(oldfilePath);
                if (r)
                {
                    using (FileStream fileStream = System.IO.File.OpenRead(oldfilePath))
                    {
                        var fileName = Path.GetFileName(oldfilePath);

                        file = new Microsoft.AspNetCore.Http.FormFile(
                            baseStream: fileStream,
                            baseStreamOffset: 0,
                            length: fileStream.Length,
                            name: fileName,
                            fileName: oldfilePath
                        );
                    }
                }
                else
                {
                    string path = root + "/Images/MSS/no-image.svg";
                    using (FileStream fileStream = System.IO.File.OpenRead(path))
                    {
                        file = new Microsoft.AspNetCore.Http.FormFile(
                            baseStream: fileStream,
                            baseStreamOffset: 0,
                            length: fileStream.Length,
                            name: "no-image",
                            fileName: path
                        );
                    }
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg);
            }
            finally { }

            return file;
        }

        public Microsoft.AspNetCore.Http.IFormFile? GetImage2(string oldfilePath, string root)
        {
            Microsoft.AspNetCore.Http.FormFile? file = null;
            try
            {
                Console.WriteLine(oldfilePath);

                var r = File.Exists(oldfilePath);

                if (r)
                {
                    using (var fileStream = System.IO.File.OpenRead(oldfilePath))
                    {
                        var fileName = Path.GetFileName(oldfilePath);
                        Console.WriteLine("Exists " + fileName);
                        var fileContent = new MemoryStream();
                        fileStream.CopyTo(fileContent);
                        fileContent.Position = 0;
                        file = new FormFile(fileContent, 0, fileContent.Length, fileName, fileName);
                    }
                }
                else
                {
                    String _path = root + "/Images/MSS/no-image.svg";

                    var fileStream = System.IO.File.OpenRead(_path);
                    var fileName = Path.GetFileName(_path);
                    Console.WriteLine("Does Not Exist " + fileName);
                    var fileContent = new MemoryStream();
                    fileStream.CopyTo(fileContent);
                    fileContent.Position = 0;
                    file = new FormFile(fileContent, 0, fileContent.Length, "no-image", fileName);
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg);
            }

            return file;
        }

        public Microsoft.AspNetCore.Http.IFormFile? GetImage3(string oldfilePath, string root)
        {
            Microsoft.AspNetCore.Http.FormFile? file = null;
            try
            {
                Console.WriteLine(oldfilePath);

                String? dir = Path.GetDirectoryName(oldfilePath);

                Console.WriteLine("Root is " + root);
                Console.WriteLine("Dir is " + dir);
                Console.WriteLine("Path is " + oldfilePath);
                Console.WriteLine("Full Path is " + root + oldfilePath);

                if (System.IO.File.Exists(oldfilePath))
                {
                    using (
                        System.IO.FileStream objFStream = new System.IO.FileStream(
                            oldfilePath,
                            System.IO.FileMode.Open,
                            FileAccess.Read
                        )
                    )
                    {
                        var fileName = Path.GetFileName(oldfilePath);
                        Console.WriteLine("Exists " + fileName);
                        byte[] bytRead = new byte[(int)objFStream.Length];
                        var fileContent = new MemoryStream();
                        while (objFStream.Read(bytRead, 0, (int)objFStream.Length) > 0)
                        {
                            objFStream.CopyTo(fileContent);
                        }
                        //after it reads then we copy stream to FormFile
                        fileContent.Position = 0;
                        file = new FormFile(fileContent, 0, fileContent.Length, fileName, fileName);

                        objFStream.Close();
                        objFStream.Dispose();
                    }
                }
                else
                {
                    String _path = root + "/Images/MSS/no-image.svg";
                    using (
                        System.IO.FileStream objFStream = new System.IO.FileStream(
                            _path,
                            System.IO.FileMode.Open,
                            FileAccess.Read
                        )
                    )
                    {
                        var fileName = Path.GetFileName(_path);
                        Console.WriteLine("Does Not Exist " + fileName);

                        byte[] bytRead = new byte[(int)objFStream.Length];
                        var fileContent = new MemoryStream();
                        while (objFStream.Read(bytRead, 0, (int)objFStream.Length) > 0)
                        {
                            objFStream.CopyTo(fileContent);
                        }
                        //after it reads then we copy stream to FormFile
                        fileContent.Position = 0;
                        file = new FormFile(fileContent, 0, fileContent.Length, fileName, fileName);

                        objFStream.Close();
                        objFStream.Dispose();
                    }
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg);
            }
            finally { }

            return file;
        }

        public void SaveImage(
            Microsoft.AspNetCore.Http.IFormFile file,
            string FolderName,
            string rootPath,
            string encodedImageName
        )
        {
            String savePath;
            if (file != null)
            {
                String fileName = Base64Encode(file.FileName);
                try
                {
                    if (string.IsNullOrWhiteSpace(rootPath))
                    {
                        rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }

                    var r = Directory.CreateDirectory(rootPath);
                    if (r.Exists)
                    {
                        Console.WriteLine($"Root Path is {rootPath}");
                        String path =
                            rootPath
                            + $"/Images/{FolderName}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/"
                            + fileName;
                        if (
                            !System.IO.File.Exists(
                                rootPath
                                    + $"/Images/{FolderName}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/"
                            )
                        )
                        {
                            System.IO.Directory.CreateDirectory(
                                rootPath
                                    + $"/Images/{FolderName}/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/"
                            );
                        }
                        if (System.IO.File.Exists(path))
                        {
                            Console.WriteLine($"File Path exists already {path} ");
                            savePath = path;
                        }
                        else
                        {
                            Console.WriteLine($"New File Path is {path} ");
                            using (var filestream = new FileStream(path, FileMode.CreateNew))
                            {
                                file.CopyTo(filestream);
                                savePath = path;
                            }
                        }
                    }
                    else
                    {
                        savePath = rootPath + "/Images/MSS/no-image.svg";
                    }
                }
                catch (Exception m)
                {
                    Console.WriteLine(m);
                    savePath = rootPath + "/Images/MSS/no-image.svg";
                }
                finally
                {
                    Console.WriteLine($"Continueing at {DateTime.UtcNow.AddHours(-4)}");
                }
            }
            else
            {
                savePath = rootPath + "/Images/MSS/no-image.svg";
            }
        }

        public async Task SaveBusinessImage(
            Microsoft.AspNetCore.Http.IFormFile file,
            string rootPath,
            string encodedImageName
        )
        {
            try
            {
                await voidTask(1);
                if (file.Length > 0)
                {
                    string path =
                        rootPath
                        + "/Images/Businesses"
                        + $"/{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}/";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (FileStream fileStream = System.IO.File.Create(path + encodedImageName))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg);
            }
        }

        public async Task SaveProductImage(
            Microsoft.AspNetCore.Http.IFormFile file,
            string rootPath,
            string encodedImageName
        )
        {
            try
            {
                await voidTask(1);
                if (file.Length > 0)
                {
                    string path =
                        rootPath
                        + "/Images/Products"
                        + $"/{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}/";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (FileStream fileStream = System.IO.File.Create(path + encodedImageName))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg);
            }
        }

        public async Task SaveServiceImage(
            Microsoft.AspNetCore.Http.IFormFile file,
            string rootPath,
            string encodedImageName
        )
        {
            try
            {
                await voidTask(1);
                if (file.Length > 0)
                {
                    string path =
                        rootPath
                        + "/Images/Services"
                        + $"/{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}/";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (FileStream fileStream = System.IO.File.Create(path + encodedImageName))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg);
            }
        }

        private async Task voidTask(int num)
        {
            await Task.Factory.StartNew(() => Thread.Sleep(num));
        }

        public async Task<bool> WaitTask(int num)
        {
            await voidTask(num);
            return true;
        }

        private int ConvertStringToInt(string stringToConvert)
        {
            int stringValue = 0;
            foreach (char c in stringToConvert)
            {
                int conv = Convert.ToInt32(c);
                stringValue += conv;
                Console.WriteLine($"{c} is {conv}: added to make {stringValue}");
            }
            return stringValue;
        }

        public ControllerResponses? responses;

        public Token GetToken()
        {
            return new Token();
        }

        public ControllerResponses GetControllerResponses()
        {
            return new ControllerResponses();
        }

        public string Encrypt(string emailOrusername, string whatIsBeingEncrypted)
        {
            return EncryptDecrypt.EncryptString(emailOrusername, whatIsBeingEncrypted);
        }

        public string Encrypt(string whatIsBeingEncrypted)
        {
            return EncryptDecrypt.EncryptString(whatIsBeingEncrypted);
        }

        public string Dencrypt(string emailOrusername, string whatIsBeingDecrypted)
        {
            return EncryptDecrypt.DecryptString(emailOrusername, whatIsBeingDecrypted);
        }

        public string Dencrypt(string whatIsBeingDecrypted)
        {
            return EncryptDecrypt.DecryptString(whatIsBeingDecrypted);
        }

        public string Base64Encode(string plainText)
        {
            return EncryptDecrypt.Base64Encode(plainText);
        }

        public string Base64Decode(string base64EncodedData)
        {
            return EncryptDecrypt.Base64Decode(base64EncodedData);
        }

        public string UrlEncode(string urlstring)
        {
            return HttpUtility.UrlEncode(urlstring);
        }

        public string UrlDecode(string urlstring)
        {
            return HttpUtility.UrlDecode(urlstring);
        }

        public string v2_Encrypt(string text)
        {
            return EncryptDecrypt.v2_EncryptText(text);
        }

        public static byte[] _v2_Encrypt(string text)
        {
            return EncryptDecrypt.v2_EncryptText_ToBytes(text);
        }

        public static string v2_Encrypt_ToString(string text)
        {
            return EncryptDecrypt.v2_EncryptText(text);
        }

        public string v2_Decrypt(string? text)
        {
            return EncryptDecrypt.v2_DecryptText(text);
        }

        public static string v2_Decrypt_ToString(string? text)
        {
            return EncryptDecrypt.v2_DecryptText(text);
        }

        public static string _v2_Decrypt(byte[]? text)
        {
            return EncryptDecrypt.v2_DecryptText_FromBytes(text);
        }

        [XmlRoot(ElementName = "XML_EncryptDecrypt")]
        public class XML_EncryptDecrypt
        {
            [XmlElement("Public")]
            public RSAParameters Public { get; set; }

            [XmlElement("Private")]
            public RSAParameters Private { get; set; }
        }

        public class XMLTYPE<T>
        {
            [XmlAttribute]
            public T? Value { get; set; }
        }

        public class EncryptDecrypt
        {
            static int KeyLength;

            private static void CheckEncyptionSize(string key)
            {
                var key1 = Encoding.UTF8.GetBytes(key);
                //myAes.Key = Key; //ERROR
                KeySizes[] ks = Aes.Create().LegalKeySizes;
                foreach (KeySizes item in ks)
                {
                    KeyLength = (int)key1.LongLength;
                    Console.WriteLine(
                        "Legal min key size = " + item.MinSize + $" {key1.LongLength}"
                    );
                    Console.WriteLine(
                        "Legal max key size = " + item.MaxSize + $" {key1.LongLength}"
                    );
                }
            }

            private static T? Deserialize<T>(string data) where T : class
            {
                if (data == null)
                {
                    return null;
                }
                else
                {
                    if (data.Trim().Length == 0)
                    {
                        return null;
                    }

                    var ser = new XmlSerializer(typeof(T));

                    using (var sr = new StringReader(data))
                    {
                        return (T?)ser.Deserialize(sr);
                    }
                }
            }

            public static string v2_EncryptText(string text)
            {
                string Return = "";
                try
                {
                    using (var rsa = new RSACryptoServiceProvider() { KeySize = 1024 * 2 })
                    {
                        var path = Directory.GetCurrentDirectory();
                        RSAParameters? pub;
                        RSAParameters? priv;
                        //    first we read from the xml to check if RSAParams exist already
                        using (
                            Stream reader = new FileStream(
                                path + "/Content/Encrypt_Decrypt.xml",
                                FileMode.Open
                            )
                        )
                        {
                            var xs = new System.Xml.Serialization.XmlSerializer(
                                typeof(XML_EncryptDecrypt)
                            );
                            //get the object back from the stream
                            var ret = (XML_EncryptDecrypt?)xs.Deserialize(reader);
                            pub = ret?.Public;
                            priv = ret?.Private;
                            // Write out the properties of the object.

                            if (pub == null)
                            {
                                Console.WriteLine("It is empty");
                                pub = rsa.ExportParameters(false);
                                priv = rsa.ExportParameters(true);
                                using (
                                    Stream writer = new FileStream(
                                        path + "/Content/Encrypt_Decrypt.xml",
                                        FileMode.Open,
                                        FileAccess.ReadWrite
                                    )
                                )
                                {
                                    var c = new XML_EncryptDecrypt();
                                    c.Public = (RSAParameters)pub;
                                    c.Private = (RSAParameters)priv;
                                    var xs2 = new System.Xml.Serialization.XmlSerializer(
                                        typeof(XML_EncryptDecrypt)
                                    );

                                    //get the object back from the stream
                                    xs2.Serialize(writer, c);
                                }
                            }

                            rsa.ImportParameters((RSAParameters)pub);
                            var byteData = Encoding.Unicode.GetBytes(text);
                            var encryptedData = rsa.Encrypt(byteData, false);
                            Return = Convert.ToBase64String(encryptedData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally { }
                return Return;
            }

            public static byte[] v2_EncryptText_ToBytes(string text)
            {
                byte[] Return = new byte[] { };
                try
                {
                    using (var rsa = new RSACryptoServiceProvider() { KeySize = 1024 * 2 })
                    {
                        var path = Directory.GetCurrentDirectory();
                        RSAParameters? pub;
                        RSAParameters? priv;
                        //    first we read from the xml to check if RSAParams exist already
                        using (
                            Stream reader = new FileStream(
                                path + "/Content/Encrypt_Decrypt.xml",
                                FileMode.Open
                            )
                        )
                        {
                            var xs = new System.Xml.Serialization.XmlSerializer(
                                typeof(XML_EncryptDecrypt)
                            );
                            //get the object back from the stream
                            var ret = (XML_EncryptDecrypt?)xs.Deserialize(reader);
                            pub = ret?.Public;
                            priv = ret?.Private;
                            // Write out the properties of the object.

                            if (pub == null)
                            {
                                Console.WriteLine("It is empty");
                                pub = rsa.ExportParameters(false);
                                priv = rsa.ExportParameters(true);
                                using (
                                    Stream writer = new FileStream(
                                        path + "/Content/Encrypt_Decrypt.xml",
                                        FileMode.Open,
                                        FileAccess.ReadWrite
                                    )
                                )
                                {
                                    var c = new XML_EncryptDecrypt();
                                    c.Public = (RSAParameters)pub;
                                    c.Private = (RSAParameters)priv;
                                    var xs2 = new System.Xml.Serialization.XmlSerializer(
                                        typeof(XML_EncryptDecrypt)
                                    );

                                    //get the object back from the stream
                                    xs2.Serialize(writer, c);
                                }
                            }

                            rsa.ImportParameters((RSAParameters)pub);
                            var byteData = Encoding.Unicode.GetBytes(text);
                            var encryptedData = rsa.Encrypt(byteData, false);
                            Return = encryptedData;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally { }
                return Return;
            }

            public static string v2_DecryptText(string? text)
            {
                if (text == null || text == "" || text?.Length < 100)
                {
                    return "Empty, Null, or non encrypted string";
                }
                string Return = "";
                try
                {
                    using (var rsa = new RSACryptoServiceProvider() { KeySize = 1024 * 2 })
                    {
                        var path = Directory.GetCurrentDirectory();
                        RSAParameters? pub;
                        RSAParameters? priv;
                        //first we read from the xml to check if RSAParams exist already
                        using (
                            Stream reader = new FileStream(
                                path + "/Content/Encrypt_Decrypt.xml",
                                FileMode.Open
                            )
                        )
                        {
                            var xs = new System.Xml.Serialization.XmlSerializer(
                                typeof(XML_EncryptDecrypt)
                            );
                            //get the object back from the stream
                            var ret = (XML_EncryptDecrypt?)xs.Deserialize(reader);
                            pub = ret?.Public;
                            priv = ret?.Private;
                            // Write out the properties of the object.

                            var privateRSA = (RSAParameters?)priv;
                            if (privateRSA == null)
                            {
                                privateRSA = rsa.ExportParameters(true);
                            }

                            rsa.ImportParameters((RSAParameters)privateRSA);
                            byte[] bi = Convert.FromBase64String(text.Trim());
                            byte[] bdecr = rsa.Decrypt(bi, false);
                            Return = Encoding.Unicode.GetString(bdecr);
                            Console.WriteLine(_break + $"Decrypted item is {Return}" + _break);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally { }
                return Return;
            }

            public static string v2_DecryptText_FromBytes(byte[]? text)
            {
                string Return = "";
                if (text == null || text.Length < 1)
                {
                    return Return;
                }
                try
                {
                    using (var rsa = new RSACryptoServiceProvider() { KeySize = 1024 * 2 })
                    {
                        var path = Directory.GetCurrentDirectory();
                        RSAParameters? pub;
                        RSAParameters? priv;
                        //first we read from the xml to check if RSAParams exist already
                        using (
                            Stream reader = new FileStream(
                                path + "/Content/Encrypt_Decrypt.xml",
                                FileMode.Open
                            )
                        )
                        {
                            var xs = new System.Xml.Serialization.XmlSerializer(
                                typeof(XML_EncryptDecrypt)
                            );
                            //get the object back from the stream
                            var ret = (XML_EncryptDecrypt?)xs.Deserialize(reader);
                            pub = ret?.Public;
                            priv = ret?.Private;
                            // Write out the properties of the object.

                            var privateRSA = (RSAParameters?)priv;
                            if (privateRSA == null)
                            {
                                privateRSA = rsa.ExportParameters(true);
                            }

                            rsa.ImportParameters((RSAParameters)privateRSA);
                            byte[] bdecr = rsa.Decrypt(text ?? new byte[] { }, false);
                            Return = Encoding.Unicode.GetString(bdecr ?? new byte[] { });
                            Console.WriteLine(_break + $"Decrypted item is {Return}" + _break);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally { }
                return Return;
            }

            public static string Base64Encode(string plainText)
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                string result = System.Convert.ToBase64String(plainTextBytes);
                Console.WriteLine($"{result}");
                return result;
            }

            public static string Base64Decode(string base64EncodedData)
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                string result = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                Console.WriteLine($"{result}");
                return result;
            }

            public static string EncryptString(string key, string plainText)
            {
                CheckEncyptionSize(PUBLIC_ENCRYPTKEY);
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (
                            CryptoStream cryptoStream = new CryptoStream(
                                (Stream)memoryStream,
                                encryptor,
                                CryptoStreamMode.Write
                            )
                        )
                        {
                            using (
                                StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream)
                            )
                            {
                                streamWriter.Write(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(array);
            }

            public static string EncryptString(string objectToEncrypt)
            {
                CheckEncyptionSize(PUBLIC_ENCRYPTKEY);
                byte[] iv = new byte[16];

                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(PUBLIC_ENCRYPTKEY);
                    aes.IV = iv;
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (
                            CryptoStream cryptoStream = new CryptoStream(
                                (Stream)memoryStream,
                                encryptor,
                                CryptoStreamMode.Write
                            )
                        )
                        {
                            using (
                                StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream)
                            )
                            {
                                streamWriter.Write(objectToEncrypt);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(array);
            }

            public static string DecryptString(string key, string cipherText)
            {
                CheckEncyptionSize(PRIVATE_ENCRYPTKEY);
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (
                            CryptoStream cryptoStream = new CryptoStream(
                                (Stream)memoryStream,
                                decryptor,
                                CryptoStreamMode.Read
                            )
                        )
                        {
                            using (
                                StreamReader streamReader = new StreamReader((Stream)cryptoStream)
                            )
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }

            public static string DecryptString(string objectToDecypt)
            {
                string Return = "";
                CheckEncyptionSize(PUBLIC_ENCRYPTKEY);
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(objectToDecypt);
                try
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = Encoding.UTF8.GetBytes(PUBLIC_ENCRYPTKEY);

                        aes.IV = iv;
                        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                        {
                            using (
                                CryptoStream cryptoStream = new CryptoStream(
                                    (Stream)memoryStream,
                                    decryptor,
                                    CryptoStreamMode.Read
                                )
                            )
                            {
                                using (
                                    StreamReader streamReader = new StreamReader(
                                        (Stream)cryptoStream
                                    )
                                )
                                {
                                    Return = streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                catch (Exception msg)
                {
                    Console.WriteLine(msg);
                }
                return Return;
            }
        }

        /// <summary>
        /// Helps you get a jwt token with your email
        /// </summary>
        public class Token
        {
            private string email;
            private string personid;

            public string Email
            {
                get => email;
            }
            public string PersonId
            {
                get => personid;
            }

            public Token()
            {
                email = "";
                personid = "";
            }

            public Token(string _em, string _personid)
            {
                this.email = _em;
                this.personid = _personid;
            }

            /// <summary>
            /// This generates a token based on the user's email or username and assigns their role
            /// </summary>
            /// <param name="email_or_username"></param>
            /// <returns></returns>
            public string Generate_MINTSOUP_JWTtoken(
                string PersonId,
                string email,
                string? username,
                string RequestIssuerUrl,
                string RequestAudienceUrl
            )
            {
                var MINTSOUP_securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes($"MINTSOUP|BY|SENDES")
                );
                var token_credentials = new SigningCredentials(
                    MINTSOUP_securityKey,
                    SecurityAlgorithms.HmacSha256
                );
                var myclaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Iss, "MINTSOUP"),
                    new Claim("PersonId", PersonId), //TODO - we should check if the user's credentials say they are admin/viewer/guest/or none
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim("Username", username ?? PersonId),
                    new Claim(
                        JwtRegisteredClaimNames.AuthTime,
                        DateTime.UtcNow.AddHours(-4).ToString()
                    ),
                    new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddHours(-3).ToString())
                };

                var tokenOptions = new JwtSecurityToken(
                    issuer: RequestIssuerUrl,
                    audience: RequestAudienceUrl,
                    claims: myclaims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: token_credentials
                );
                return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            }

            public JwtSecurityToken? Extract_JWT_claims(string MSToken)
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(MSToken);
                return token;
            }
        }

        public class ControllerResponses
        {
            private enum WhichControllerIsIt
            {
                Get,
                Create,
                Check
            }

            private static WhichControllerIsIt distinction { get; set; }
            private readonly static string OkString =
                $"\n\n\t\tThe request was found (true) at '{DateTime.Now}' due to a check for: \n\t\t";
            private readonly static string CreatedString =
                $"\n\n\t\tThe request was created successfully (true) at '{DateTime.Now}' for: \n\t\t";
            private readonly static string PartiallyCreatedString =
                $"\n\n\t\tThe request was partially created (parts not saved) at '{DateTime.Now}' for: \n\t\t";
            private readonly static string BadRequestString =
                $"\n\n\t\tThere was a bad request at '{DateTime.Now}' for: \n\t\t";
            private readonly static string NotFoundString =
                $"\n\n\t\tThe request was not found at '{DateTime.Now}' for: \n\t\t";
            private readonly static string NotCreatedString =
                $"\n\n\t\tThe request was not created (false) at '{DateTime.Now}' for: \n\t\t";
            private readonly static string ConflictString =
                $"\n\n\t\tThere was a conflict during the request at '{DateTime.Now}' for: \n\t\t";

            public string ConflictRequest(object o, object? forwhatAction)
            {
                var a = "";
                if (forwhatAction == null)
                {
                    a = string.Join(ConflictString, o);
                }
                else
                {
                    a = string.Join(ConflictString, $"{forwhatAction} as {o}");
                }
                Console.WriteLine(a);
                return a;
            }

            public string BadRequest(object o, object? forwhatAction)
            {
                var a = "";
                if (forwhatAction == null)
                {
                    a = string.Join(BadRequestString, o);
                }
                else
                {
                    a = string.Join(BadRequestString, $"{forwhatAction} as {o}");
                }

                Console.WriteLine(a);
                return a;
            }

            public string NotFoundRequest(object o, object? forwhatAction)
            {
                var a = "";
                if (forwhatAction == null)
                {
                    a = string.Join(NotFoundString, o);
                }
                else
                {
                    a = string.Join(NotFoundString, $"{forwhatAction} as {o}");
                }
                Console.WriteLine(a);
                return a;
            }

            public string NotCreatedRequest(object o, object? forwhatAction)
            {
                var a = "";
                if (forwhatAction == null)
                {
                    a = string.Join(NotCreatedString, o);
                }
                else
                {
                    a = string.Join(NotCreatedString, $"{forwhatAction} as {o}");
                }
                Console.WriteLine(a);
                return a;
            }

            public string CreatedRequest(object o, object? forwhatAction)
            {
                var a = "";
                if (forwhatAction == null)
                {
                    a = string.Join(CreatedString, o);
                }
                else
                {
                    a = string.Join(CreatedString, $"{forwhatAction} as {o}");
                }
                Console.WriteLine(a);
                return a;
            }

            public string PartiallyCreatedRequest(object o, object? forwhatAction)
            {
                var a = "";
                if (forwhatAction == null)
                {
                    a = string.Join(PartiallyCreatedString, o);
                }
                else
                {
                    a = string.Join(PartiallyCreatedString, $"{forwhatAction} as {o}");
                }
                Console.WriteLine(a);
                return a;
            }

            public string OkRequest(object o, object? forwhatAction)
            {
                var a = "";
                if (forwhatAction == null)
                {
                    a = string.Join(OkString, o);
                }
                else
                {
                    a = string.Join(OkString, $"{forwhatAction} as {o}");
                }
                Console.WriteLine(a);
                return a;
            }
        }

        public class ConvertTypes
        {
            public T ConvertEnum<T>(int i) where T : struct, IConvertible
            {
                return (T)(object)i;
            }
        }
    }
}
