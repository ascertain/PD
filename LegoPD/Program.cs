using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using NUnit.Framework;
using System.IO;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var websiteName = "https://qa-playdays.lego.com/";
            string subject = "Regress Test for  " + websiteName;
            string str = "<table border=1 ><caption>Play Day Regress Test</caption>";
            str += "<tr><th style='width:250px'>Test Case </th><th style='width:250px;'>Result</th></tr>";

            //Console.WriteLine("\n\tPlease Enter the Username : ");
            //string username = "HCLLEGO";
            string username = "vashiadi";
            // Console.WriteLine("\n\tPlease Enter the Password : ");
            //string password = "legohcl";
            string password = "lego@123";
            string tittle = "Play Days";

            //FirefoxProfile profile = new FirefoxProfile();
            //Proxy proxy = new Proxy();
            //proxy.IsAutoDetect = true;
            //profile.SetProxyPreferences(proxy);
            //System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", @"D:\LegoPD\packages\chromedriver.exe");
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\lib";
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("--test-type");
            IWebDriver driver = new ChromeDriver((path), option);
            //IWebDriver driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(websiteName);
            try
            {
                Assert.AreEqual(driver.Title, tittle);
                Console.WriteLine("\n\tTest Case 1 Passed: Tittle Matched");
                str += "<tr><td>Test Case 1 : Tittle Matched</td><td> Pass" + "</td></tr>";
            }
            catch
            {
                Console.WriteLine("\tTest Case 1 Failed: Tittle Not Matched");
                str += "<tr><td>Test Case 1 : Tittle Matched</td><td> Fail" + "</td></tr>";
            }
            //Actions builder = new Actions(driver);
            //builder.SendKeys(Keys.Enter);

            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("a[data-uitest=login-link]")));
            driver.FindElement(By.CssSelector("a[data-uitest=login-link]")).Click();




            //---------------------------Valid User(both username and password are provided correctly)-----------------------------------------------
            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    driver.SwitchTo().Frame("legoid-iframe");
                    Thread.Sleep(5000);

                    driver.FindElement(By.CssSelector("#fieldUsername")).SendKeys(username);
                    driver.FindElement(By.Id("fieldPassword")).SendKeys(password);
                    driver.FindElement(By.Id("buttonSubmitLogin")).Click();
                    Thread.Sleep(5000);
                    driver.Navigate().Refresh();
                    IWebElement anchor = driver.FindElement(By.CssSelector("a[data-uitest=edit-profile-link]"));
                    string innerText = anchor.Text;
                    Assert.AreEqual(innerText, username.ToUpper());
                    Console.WriteLine("\n\tTest Case 2 Passed: Successfully Logged In");
                    str += "<tr><td>Test Case 2 : Successfully Logged In</td><td> Pass" + "</td></tr>";
                }
            }
            catch
            {
                Console.WriteLine("\n\tTest Case 2 Failed : User Not Logged In");
                str += "<tr><td>Test Case 2 : Successfully Logged In using Lego ID</td><td> Fail " + "</td></tr>";
            }


            //-------------------------On Entering the wrong school code-----------------------------------------------------------------------

            //driver.Navigate().Back();
            //Thread.Sleep(2000);
            try
            {
                wait.Until(d => driver.FindElement(By.CssSelector("#codeValue")).Displayed);
                driver.FindElement(By.CssSelector("#codeValue")).SendKeys("adsf");
                driver.FindElement(By.CssSelector(".btn.default-btn.login-home.yellow-btn")).Click();

                Thread.Sleep(1000);
                IWebElement model = driver.FindElement(By.Id("modal-body"));
                if (model.Displayed)
                {
                    Console.WriteLine("\n\tTest Case 3 Passed : Wrong School code popup message detected");
                    str += "<tr><td>Test Case 3 : Wrong School code popup message detected</td><td> Pass" + "</td></tr>";
                }
            }
            catch
            {
                Console.WriteLine("\n\tTest Case 3 Failed : Wrong School code Not detected");
                str += "<tr><td>Test Case 3 : Wrong School code popup message detected</td><td> Fail" + "</td></tr>";
            }


            //-------------------------On Entering the correct school code-----------------------------------------------------------------------
            try
            {

                driver.Navigate().Refresh();
                Thread.Sleep(2000);
                wait.Until(d => driver.FindElement(By.CssSelector("#codeValue")).Displayed);
                driver.FindElement(By.CssSelector("#codeValue")).SendKeys("S12-345-678");
                driver.FindElement(By.CssSelector(".btn.default-btn.login-home.yellow-btn")).Click();
                Thread.Sleep(2000);

                var registration = driver.FindElement(By.CssSelector(".ng-pristine.ng-invalid.ng-invalid-required.ng-valid-pattern.ng-valid-maxlength.ng-valid-minlength.ng-valid-email h2"));
                string registrationText = registration.Text;
                Assert.AreEqual(registrationText, "REGISTRATION");

                Thread.Sleep(2000);
                Console.WriteLine("\n\tTest Case 4 Passed : Correct school code is working");
                str += "<tr><td>Test Case 4 : Correct school code is working</td><td> Pass" + "</td></tr>";
            }
            catch
            {
                Console.WriteLine("\n\tTest Case 4 Failed : Correct school code is Not working");
                str += "<tr><td>Test Case 4 : Correct school code is working</td><td> Fail" + "</td></tr>";
            }

            //-------------------------filling the form with wrong details--------------------------------------------------------------------------

            try
            {
                StreamReader reader = new StreamReader(path + "\\wronguser.txt");

                string wstr = reader.ReadToEnd();
                reader.Close();

                string[] line = wstr.Split('\n');

                string fname = line[0];
                string sname = line[1];
                string address1 = line[2];
                string address2 = line[3];
                string city = line[4];
                string postal = line[5];
                string DOB = line[6];
                string classname = line[7];
                string parent1 = line[8];
                string email1 = line[9];
                string phone1 = line[10];
                string parent2 = line[11];
                string email2 = line[12];
                string phone2 = line[13];
                driver.FindElement(By.CssSelector("input[placeholder='First Name']")).SendKeys(fname);
                driver.FindElement(By.CssSelector("input[placeholder='Last Name']")).SendKeys(sname);
                driver.FindElement(By.CssSelector("input[placeholder='Address 1']")).SendKeys(address1);
                driver.FindElement(By.CssSelector("input[placeholder='Address 2']")).SendKeys(address2);
                driver.FindElement(By.CssSelector("input[placeholder='City']")).SendKeys(city);
                driver.FindElement(By.CssSelector("input[placeholder='Postal Code']")).SendKeys(postal);
                driver.FindElement(By.CssSelector("input[placeholder='DD-MM-YYYY']")).SendKeys(DOB);
                new SelectElement(driver.FindElement(By.CssSelector("select[name='schoolName']"))).SelectByText("School1");
                driver.FindElement(By.CssSelector("input[placeholder='Class']")).SendKeys(classname);
                driver.FindElement(By.CssSelector("input[name='firstParentsName']")).SendKeys(parent1);
                driver.FindElement(By.CssSelector("input[name='firstparentsemail']")).SendKeys(email1);
                driver.FindElement(By.CssSelector("input[name='firstParentsNumber']")).SendKeys(phone1);
                driver.FindElement(By.CssSelector("input[name='secondParentsname']")).SendKeys(parent2);
                driver.FindElement(By.CssSelector("input[name='secondparentsemail']")).SendKeys(email2);
                driver.FindElement(By.CssSelector("input[name='secondParentsNumber']")).SendKeys(phone2);

                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> classList = driver.FindElements(By.ClassName("error-message"));
                //IWebElement [] Errormessage = driver.FindElements(By.CssSelector(".error-message"));
                //IWebElement [] message = driver.FindElements(By.CssSelector(".error-message"));
                if (classList.Count() != 0)
                {
                    Console.WriteLine("\n\tTest Case 5 Passed : Check Is Invalid message is displayed ");
                    str += "<tr><td>Test Case 5 : Check Is Invalid message is displayed </td><td> Pass" + "</td></tr>";
                }
                else
                {
                    Console.WriteLine("\n\tTest Case 5 Passed : Error Message on registration page Not detected");
                    str += "<tr><td>Test Case 5 : Check Is Invalid message is displayed </td><td> Fail" + "</td></tr>";
                }
            }
            catch
            {
                Console.WriteLine("\n\tTest Case 5 Passed : Error Message on registration page Not detected");
                str += "<tr><td>Test Case 5 : Check Is Invalid message is displayed </td><td> Fail" + "</td></tr>";
            }

            //--------------------------------filling the form with correct details------------------------------------------------------------------------

            //StreamReader reader = new StreamReader(path+"\\correctuser.txt");

            //string str = reader.ReadToEnd();
            //reader.Close();

            //string[] line = str.Split('\n');

            //string fname = line[0];
            //string sname = line[1];
            //string address1 = line[2];
            //string address2 = line[3];
            //string city = line[4];
            //string postal = line[5];
            //string DOB = line[6];
            //string classname = line[7];
            //string parent1 = line[8];
            //string email1 = line[9];
            //string phone1 = line[10];
            //string parent2 = line[11];
            //string email2 = line[12];
            //string phone2 = line[13];
            //driver.FindElement(By.CssSelector("input[placeholder='First Name']")).SendKeys(fname);
            //driver.FindElement(By.CssSelector("input[placeholder='Last Name']")).SendKeys(sname);
            //driver.FindElement(By.CssSelector("input[placeholder='Address 1']")).SendKeys(address1);
            //driver.FindElement(By.CssSelector("input[placeholder='Address 2']")).SendKeys(address2);
            //driver.FindElement(By.CssSelector("input[placeholder='City']")).SendKeys(city);
            //driver.FindElement(By.CssSelector("input[placeholder='Postal Code']")).SendKeys(postal);
            //driver.FindElement(By.CssSelector("input[placeholder='DD-MM-YYYY']")).SendKeys(DOB);
            //new SelectElement(driver.FindElement(By.CssSelector("select[name='schoolName']"))).SelectByText("School1");
            //driver.FindElement(By.CssSelector("input[placeholder='Class']")).SendKeys(classname);
            //driver.FindElement(By.CssSelector("input[name='firstParentsName']")).SendKeys(parent1);
            //driver.FindElement(By.CssSelector("input[name='firstparentsemail']")).SendKeys(email1);
            //driver.FindElement(By.CssSelector("input[name='firstParentsNumber']")).SendKeys(phone1);
            //driver.FindElement(By.CssSelector("input[name='secondParentsname']")).SendKeys(parent2);
            //driver.FindElement(By.CssSelector("input[name='secondparentsemail']")).SendKeys(email2);
            //driver.FindElement(By.CssSelector("input[name='secondParentsNumber']")).SendKeys(phone2);




            str += "</table>";
            //Console.WriteLine(str);
            var fileName = "result_" + Guid.NewGuid().ToString() + ".html";
            StreamWriter sw = new StreamWriter(path + @"\" + fileName);
            sw.WriteLine(DateTime.Now.ToString() + str);
            sw.Flush();
            sw.Close();

            driver.Navigate().GoToUrl("file:///" + path + @"\" + fileName);



            /* try
             {
                 Process p = new Process();
                 var startInfo = new ProcessStartInfo("iexplorer.exe");

                 startInfo.Arguments = ".\\" + fileName;
                 p.StartInfo = startInfo;
                 p.Start();              
             }
             catch (Exception ex)
             {
                 throw ex;
             }*/

        }
    }

}