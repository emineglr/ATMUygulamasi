namespace ATMUygulamasi
{
    public class Program
    {
        static UserManager userManager = new UserManager();
        static TransactionManager transactionManager = new TransactionManager();
        static FraudManager fraudManager = new FraudManager();

        public static void Main(string[] args)
        {
            CreateDummyRecord();
            do
            {

                Console.Write("Kullanıcı adınızı giriniz yada gün sonu için (9) basınız: ");
                string username = Console.ReadLine();
                if (username == "9")
                {

                    Console.Write("Şifrenizi giriniz: ");
                    string password = Console.ReadLine();
                    if (password == "123456")
                        Report();
                    else
                    {
                        Console.WriteLine("Yetkili girişi hatalı!");
                    }
                }
                else
                {

                    User user = userManager.Find(username);
                    if (user != null)
                    {
                        Console.WriteLine("Şifrenizi giriniz: ");
                        string password = Console.ReadLine();
                        User userLogin = userManager.Login(username, password);
                        string key = "";
                        do
                        {

                            if (userLogin != null)
                            {
                                Console.Write("Yapmak istediğiniz işlemi seçiniz \nPara Çekme(1) \nPara Yatırma(2) \nFatura Ödeme(3) \nBakiye Sorgulama(4) \nÇıkış(0)\n");
                                key = Console.ReadLine();
                                if (key == "1")
                                {
                                    MoneyPull(userLogin);
                                }
                                else if (key == "2")
                                {
                                    MoneyPush(userLogin);
                                }
                                else if (key == "3")
                                {
                                    Payment(userLogin);
                                }
                                else if (key == "4")
                                {
                                    BalanceInfo(userLogin);
                                }


                            }
                            else
                            {
                                Console.WriteLine("Kullanıcı adı yada şifre sistemde yok!");
                                Fraud fraud = new Fraud();
                                fraud.FraudType = FraudType.HataliGiriş;
                                fraud.FraudDesc = "Kullanıcı girişi hatalı";
                                fraud.CreateDate = DateTime.Now;
                                fraud.User = user;
                                fraudManager.Add(fraud);
                                break;
                            }
                        } while (key != "0");
                    }
                    else
                    {
                        Console.WriteLine("Kullanıcı bulunamadı..");
                    }

                }

            } while (true);
        }
        private static void CreateDummyRecord()
        {
            User user = new User();
            user.UserName = "emine";
            user.Password = "123";
            userManager.Add(user);

            user = new User();
            user.UserName = "Asaf";
            user.Password = "456";
            userManager.Add(user);

        }
        public static void MoneyPull(User user)
        {

            Console.Write("Çekmek istediğiniz tutarı giriniz: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal money))
            {
                Transaction transaction = new Transaction();
                transaction.OldBalance = userManager.GetBalance(user.UserId);
                if (transaction.OldBalance >= money)
                {
                    transaction.NewBalance = transaction.OldBalance - money;
                    transaction.TransactionType = TransactionType.ParaCekme;
                    transaction.User = user;
                    transaction.CreateDate = DateTime.Now;
                    transactionManager.Add(transaction);
                    userManager.UpdateBalance(transaction.NewBalance, transaction.User.UserId);
                    if (money >= 50000)
                    {
                        Fraud fraud = new Fraud();
                        fraud.FraudType = FraudType.YuksekMiktardaParaCekme;
                        fraud.FraudDesc = "50000 den fazla para çekildi - Çekilen tutar: " + money;
                        fraud.CreateDate = DateTime.Now;
                        fraud.User = user;
                        fraudManager.Add(fraud);
                    }


                }
                else
                {
                    Console.WriteLine("Bakiyeniz yeterli değil!!");
                }
            }
            else
            {
                Console.WriteLine("Tutarı yanlış girdiniz!!");

            }

        }
        public static void MoneyPush(User user)
        {
            Console.Write("Yatırmak istediğiniz tutarı giriniz: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal money))
            {
                Transaction transaction = new Transaction();
                transaction.OldBalance = userManager.GetBalance(user.UserId);
                transaction.NewBalance = transaction.OldBalance + money;
                transaction.TransactionType = TransactionType.ParaYatirma;
                transaction.User = user;
                transaction.CreateDate = DateTime.Now;
                transactionManager.Add(transaction);
                userManager.UpdateBalance(transaction.NewBalance, transaction.User.UserId);


            }
            else
            {
                Console.WriteLine("Tutarı yanlış girdiniz!!");

            }
        }

        public static void Payment(User user)
        {
            Console.WriteLine("Fatura ödemesi için Tutar giriniz: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal money))
            {
                Transaction transaction = new Transaction();
                transaction.OldBalance = userManager.GetBalance(user.UserId);
                if (transaction.OldBalance >= money)
                {
                    transaction.NewBalance = transaction.OldBalance - money;
                    transaction.TransactionType = TransactionType.OdemeYapma;
                    transaction.User = user;
                    transaction.CreateDate = DateTime.Now;
                    transactionManager.Add(transaction);
                    userManager.UpdateBalance(transaction.NewBalance, transaction.User.UserId);

                }
                else
                {
                    Console.WriteLine("Bakiyeniz yeterli değil!!");
                }
            }
            else
            {
                Console.WriteLine("Tutarı yanlış girdiniz!!");

            }
        }
        public static void BalanceInfo(User user)
        {
            Console.WriteLine("Bakiye bilgileriniz: " + user.Balance);
            Transaction transaction = new Transaction();
            transaction.OldBalance = user.Balance;
            transaction.NewBalance = user.Balance;
            transaction.TransactionType = TransactionType.BakiyeSorgulama;
            transaction.User = user;
            transaction.CreateDate = DateTime.Now;
            transactionManager.Add(transaction);


        }
        public static void Report()
        {
            Transaction transaction = new Transaction();
            string fileName = System.AppDomain.CurrentDomain.BaseDirectory + "\\EOD_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";

            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.WriteLine("-----Transaction Listesi-----");
                outputFile.WriteLine($"Kullanıcı\tEski Bakiye\tYeni Bakiye\tİşlem Tipi\tİşlem Tarihi");
                foreach (var item in transactionManager.TransactionList)
                {
                    outputFile.WriteLine($"{item.User.UserName} \t {item.OldBalance} \t {item.NewBalance} \t {item.TransactionType} \t {item.CreateDate}");
                }


                outputFile.WriteLine("-----Fraud Listesi-----");
                outputFile.WriteLine($"Kullanıcı\tFraud Tipi\tFraud Açıklaması\tİşlem Tarihi");
                foreach (var item in fraudManager.FraudList)
                {
                    outputFile.WriteLine($"{item.User.UserName} \t {item.FraudType} \t {item.FraudDesc} \t {item.CreateDate}");
                }
            }





        }

    }

}

