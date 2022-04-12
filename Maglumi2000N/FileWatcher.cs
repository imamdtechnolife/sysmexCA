using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

namespace Maglumi2000N
{
    public static class FileWatcher
    {
        private static FileSystemWatcher watcher = new FileSystemWatcher();
        public static AppDbContext _db = new AppDbContext();
        public static MAHDbContext mdb = new MAHDbContext();
        private static StringBuilder sb1 = new StringBuilder();
        private static StringBuilder sb2 = new StringBuilder();
        public static int Fflag = 0;

        public static void Startwatching()
        {
            FileWatcher.watcher.Path = Constants.DumpPath;
            FileWatcher.watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.LastAccess;
            FileWatcher.watcher.Filter = "*.txt";
            FileWatcher.watcher.Created += new FileSystemEventHandler(FileWatcher.OnChanged);
            FileWatcher.watcher.Deleted += new FileSystemEventHandler(FileWatcher.OnChanged);
            FileWatcher.watcher.EnableRaisingEvents = true;
        }
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                string PId = "";
                string InstrumentName = "";
                string code = "";
                Thread.Sleep(2000);
                string content = File.ReadAllText(e.FullPath);
                //Console.Write(content);

               //// Parse(content);
                //string[] lines = content.Split(new[] { Environment.NewLine},StringSplitOptions.None);
                //Console.WriteLine("splitted lenght first: " + lines.Length);
                // Console.WriteLine("splitted lenght second: " + lines[0].Trim().Length);
                // Console.WriteLine("splitted lenght substring: " + lines[0].Substring(0, 151));
                //foreach (var item in lines[0])
                //{
                //    Console.Write(item);
                //}
                //Console.WriteLine();
                // Console.WriteLine("splitted lenght third: " + lines[1].Trim().Length);
                //InstrumentName = lines[0].Split('|')[4];
                // Console.WriteLine(InstrumentName);
                // foreach (var item in lines)
                // {
                // Console.WriteLine(item);
                //if (item.Substring(0, 1) == "R")
                //{
                //    code = item.Split('|')[2];
                //    Console.WriteLine(code);
                //}
                //}



            }
            else
            {
                if (e.ChangeType != WatcherChangeTypes.Deleted)
                    return;
                Console.WriteLine("File Deleted: " + e.FullPath);
            }
        }
        public static void Parse(string data)
        {
            string PId = "";
            string MachineName = "";
            string code = "";
            string Pid = "";
            string value = "";
            string codde = "";
            string reportdate = "";
            string unit = "";
            string range = "";
            DateTime? Date = new DateTime?();
            try
            {
                string[] lines = data.Split(new[] { Constants.cr }, StringSplitOptions.None);
                
                MachineName = "SysmexCA600";
                Console.WriteLine(MachineName);
               // reportdate = lines[0].Split('|')[13].Substring(2, 6);
               // Console.WriteLine(reportdate);

                
                    //Date = ConvertMaglumi2000DateTime(reportdate);
                 foreach(var item in lines)
                 {
                    //Console.WriteLine(item);
                    var pipe = item.Split('|');
                    if (pipe[0].Substring(pipe[0].Length - 1) == "O")
                    {
                        Pid = pipe[2].TrimStart(new char[] { '0' });
                        Console.WriteLine("Pid==>" + Pid);

                    }
                    else if(pipe[0].Substring(pipe[0].Length-1)=="Q")
                    {
                        Fflag = 1;
                         Pid = pipe[2].Split('^')[1];
                        var li = GetTestCodes(Pid);
                        sb1.Clear();
                        Task.Factory.StartNew((Action)(() => SendTestOrder(li, Pid)));
                    }
                    else if (pipe[0].Substring(pipe[0].Length - 1) == "R")
                    {
                        Fflag = 0;
                        code = pipe[2].Split('^')[4];
                        value = pipe[3];
                        unit = pipe[4];
                        range = pipe[5];
                        //reportdate = pipe[12].Substring(2, 12);
                        Console.WriteLine("code==>" + code);
                        Console.WriteLine("value==>" + value);
                        Console.WriteLine("unit==>" + unit);
                        Console.WriteLine("range==>" + range);
                    }
                }
                //reportdate = lines[0].Split('R')[2].Split('|')[12].Substring(2, 12);
                if (Fflag == 0)
                {
                    //Console.WriteLine(reportdate);
                    //Date = ConvertMaglumi2000DateTime(reportdate);
                    //Console.WriteLine(Date);

                    Pid = Pid.TrimStart(new char[] { '0' });
                    DeleteExistingRecord(Pid, MachineName);


                    //if (value.Contains("."))
                    //{
                    //    if (value.Split('.')[1].Length > 2)
                    //    {
                    //        value = value.Split('.')[0] + "." + value.Split('.')[1].Substring(0, 2);
                    //    }
                    //    Console.WriteLine(value);
                    //}

                    PatientRecord pr = new PatientRecord()
                    {
                        PatientId = Pid,
                        InstrumentName = MachineName,
                        ReportDate = Date

                    };
                    _db.PatientRecords.Add(pr);
                    _db.SaveChanges();
                    ResultRecord rr = new ResultRecord()
                    {
                        PatientRecordId = pr.PatientRecordId,
                        Code = code,
                        Value = value,
                        Name = code,
                        Unit = unit,
                        Range = range,
                        ReportDate = Date

                    };
                    _db.Resultrecords.Add(rr);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (Fflag == 0)
            {
                Task.Factory.StartNew((Action)(() => FileWatcher.Execute_DataBase(Pid)));
            }

        }
        private static void DeleteExistingRecord(string pid, string machineName)
        {
            Console.WriteLine("Delete :" + pid);

            var SpecificRecod = FileWatcher._db.PatientRecords.Where(x => x.PatientId == pid && x.InstrumentName == machineName).FirstOrDefault();
            {
                if (SpecificRecod != null)
                {
                    var SpecificRecordItem = FileWatcher._db.Resultrecords.Where(x => x.PatientRecordId == SpecificRecod.PatientRecordId).ToList();
                    foreach (var item in SpecificRecordItem)
                    {
                        FileWatcher._db.Resultrecords.Remove(item);
                    }
                    FileWatcher._db.PatientRecords.Remove(SpecificRecod);
                    FileWatcher._db.SaveChanges();
                }
            }

        }

        private static void SendTestOrder(List<string> li, string Pid)
        {
            string STX = Constants.stx;
            string ETX = Constants.etx;
            string CR = Constants.cr;
            string LF = Constants.lf;
            string CS = "";
            List<string> Orderlist = new List<string>();
            //sb1.Clear();
            int i = 1;
            foreach (var item in li)
            {
                
                var item2 = "O| "+i+"|" + Pid + "||^^^" + item + "|R" + CR;
                sb1.Append(item2);
                i++;
            }

            Console.WriteLine(sb1.ToString());
            Orderlist.Clear();
            sb2.Clear();
            //Orderlist.Add(Constants.enq);
            //string Header = STX + @"1H|\^&||||||||||P||" +CR+ETX ;
            string Header =  @"H|\^&||PSWD|Maglumi 1000|||||Lis||P|E1394-97|20100323"+ CR;
            //CS = Checksum.GetCheckSumValue(Header);
            //Header = Header + CS + CR + LF;
            //Orderlist.Add(Header);
            sb2.Append(Header);
            //CS = "";
            // string Patient = STX +"2P|1" +CR+ ETX ;
            string Patient = "P|1" + CR;
            sb2.Append(Patient);
            //CS = Checksum.GetCheckSumValue(Patient);
            //Patient = Patient + CS + CR + LF;
            //Orderlist.Add(Patient);
            CS = "";
            Console.WriteLine("Order Pid: " + Pid);
            //string Order = STX + "3O|1|" + Pid + "|" + SN + "^" + DN + "^" + PN + "^^" + SMPL +"^"+ NRML + "|" + sb1.ToString() + "|R||||||N||||||||||||||Q" + CR+ETX;
            //string Order = "O|1|" + Pid + "|" + SN + "^" + DN + "^" + PN + "^^" + SMPL + "^" + NRML + "|" + sb1.ToString() + "|R||||||A||||1||||||||||O" + CR;
            ////CS = Checksum.GetCheckSumValue(Order);
            ////Order=Order+ CS + CR + LF;
            string Order = sb1.ToString();
            sb2.Append(Order);
            //Orderlist.Add(Order);
            ////CS = "";
            string lastline = "L|1|N" + CR ;
            ////CS = Checksum.GetCheckSumValue(lastline);
            ////lastline=lastline+ CS + CR + LF;
            sb2.Append(lastline);
            //**CS = Checksum.GetCheckSumValue(sb2.ToString());
            //**sb2.Append(CS);
           
            //Orderlist.Add(lastline);
            CS = "";
            ////Orderlist.Add(Constants.eot);
            Orderlist.Clear();
            Orderlist.Add(Constants.enq);
            Orderlist.Add(Constants.stx);
            Orderlist.Add(sb2.ToString());
            Orderlist.Add(Constants.etx);
            Orderlist.Add(Constants.eot);
            //Maglumi2000C.sp.Write(Constants.enq);
            //Maglumi2000C.flag = 0;
            //Thread.Sleep(2000);
            foreach (var item in Orderlist)
            {
                Maglumi2000C.sp.Write(item);
                while (true)
                {
                    if (Maglumi2000C.flag == 1)
                    {
                        Maglumi2000C.flag = 0;
                        //Maglumi2000C.sp.Write(sb2.ToString());
                        Console.WriteLine("Sending :" + item);

                        break;
                    }
                }
            }
            //while (true)
            //{
            //    if (Maglumi2000C.flag == 1)
            //    {
            //        Maglumi2000C.sp.Write(Constants.eot);
            //        break;
            //    }
            //}
            // Thread.Sleep(2000);
            //foreach(var item in Orderlist)
            //{
            //   // if(CobasE411.flag==1)
            //   // {
            //        CobasE411.flag = 0;
            //        CobasE411.sp.Write(item);
            //        Thread.Sleep(1000);
            //        while(true)
            //        {
            //        if (CobasE411.flag == 1)
            //            break;
            //        else if (item == Orderlist[Orderlist.Count - 1])
            //            break;
            //        else if (CobasE411.nflag == 1)
            //        {
            //            Console.WriteLine("Sending again");
            //            CobasE411.sp.Write(item);
            //            CobasE411.nflag = 0;
            //        }
            //        }
            //        Console.WriteLine("Sending :" + item);
            //    //}
            //}
           ////**** Maglumi2000C.sp.Write(Constants.eot);
            ////Console.WriteLine("Sending :" + Constants.eot);
            Orderlist.Clear();
            //// CobasE411.sp.Write(Constants.eot);

        }

        private static List<string> GetTestCodes(string pid)
        {

            var codeprefix = pid.TrimStart(new char[] { '0' }).Substring(0, 1);
            Console.WriteLine(codeprefix);
            var reportintId = pid.TrimStart(new char[] { '0' }).Substring(1);
            Console.WriteLine(reportintId);
            var patientid = mdb.Patients.Where(x => x.ReportIdPrefix == codeprefix && x.ReportId.ToString() == reportintId).Select(x => x.PatientId).FirstOrDefault();
            Console.WriteLine("patientId: " + patientid);
            var TestItems = mdb.TestsCosts.Where(x => x.PatientId == patientid).Select(x => x.TestId).ToList();
            foreach (var item in TestItems)
            {
                Console.WriteLine(item);
            }

            var TestNames = _db.Maglumi2000_Mappings.Where(x => TestItems.Contains(x.TestId)).Select(x => x.ParameterName).ToList();

            foreach (var item in TestNames)
            {
                Console.WriteLine("TestName : " + item);
            }
            return TestNames;

        }

        public static void Execute_DataBase(string Pid)
        {
            Console.WriteLine("Executing Sql for patientId: "+Pid);
            using (AppDbContext _dbContext = new AppDbContext())
            {
                var param1 = new SqlParameter("@pid1", Pid);
                var param2 = new SqlParameter("@pid2", Pid);
                _dbContext.Database.ExecuteSqlCommand(@"Exec [dbo].[spUpdateReportDefId] @pid1 ", parameters: new[] { param1 });
                Thread.Sleep(100);
                _dbContext.Database.ExecuteSqlCommand(@"Exec [dbo].[spUpdateSIBLTEMPLIS]  @pid2 ", parameters: new[] { param2 });
            }
        }

        public static DateTime? ConvertMaglumi2000DateTime( string datetime)
        {


            //20 08 01 12 20 16
            var year = int.Parse("20" + datetime.Substring(0, 2));
            var month = int.Parse(datetime.Substring(2, 2));
            var day = int.Parse(datetime.Substring(4, 2));
            var hour = int.Parse(datetime.Substring(6, 2));
            var min = int.Parse(datetime.Substring(8, 2));
            var sec = int.Parse(datetime.Substring(10, 2));

            return new DateTime(year, month, day, hour, min, sec);
        }
        //public DateTime rp_date(string date)
        //{
        //    return 
        //}
    }
}
