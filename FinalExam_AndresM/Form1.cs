using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace FinalExam_AndresM
{
    public partial class Form1 : Form
    {
        string connectStr = "server = INSTRUCTORIT; database = PhoneBook; user ID = ProfileUser; password = ProfileUser2019";

        List<Contact> listContacts = new List<Contact>();
        public Form1()
        {
            InitializeComponent();
        }


        //INSERT 
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            

            try
            {
                string ID = txtID.Text;
                if(ID == "")
                {
                    string sqlInsertCommand;
                    using (SqlConnection myConn = new SqlConnection(connectStr))
                    {
                        Contact addingContact = new Contact();

                        addingContact.FirstName = txtFirstName.Text;
                        addingContact.LastName = txtLastName.Text;
                        addingContact.Phone = txtPhone.Text;
                        addingContact.Email = txtEmail.Text;

                        listContacts.Add(addingContact);

                        myConn.Open();
                        sqlInsertCommand = "INSERT INTO Contact" + "(FirstName, LastName,Phone, Email) VALUES ('" + addingContact.FirstName + "','" + addingContact.LastName + "','" + addingContact.Phone + "','" + addingContact.Email + "');";
                        SqlCommand myInsertCommand = new SqlCommand(sqlInsertCommand, myConn);
                        myInsertCommand.ExecuteNonQuery();
                        myConn.Close();

                    }
                    MessageBox.Show("Contact added successfully!");
                }else
                    MessageBox.Show("Please don't add ID number");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //UPDATE

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlUpdateCommand;
                using (SqlConnection myConn = new SqlConnection(connectStr))
                {
                    Contact updateContact = new Contact();
                    updateContact.ID = int.Parse(txtID.Text);
                    updateContact.FirstName = txtFirstName.Text;
                    updateContact.LastName = txtLastName.Text;
                    updateContact.Phone = txtPhone.Text;
                    updateContact.Email = txtEmail.Text;

                    myConn.Open();
                    sqlUpdateCommand = "UPDATE Contact " + "SET FirstName = '" + updateContact.FirstName + "', LastName = '" + updateContact.LastName + "', Phone = ' "+updateContact.Phone+ "', Email = '" + updateContact.Email + "' WHERE ID =" + updateContact.ID + ";";
                    SqlCommand myUpdateCommand = new SqlCommand(sqlUpdateCommand, myConn);
                    myUpdateCommand.ExecuteNonQuery();
                    myConn.Close();
                }

                MessageBox.Show("Contact updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //DELETE
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlDeleteCommand;
                using (SqlConnection myConn = new SqlConnection(connectStr))
                {
                    Contact deleteContact = new Contact();
                    deleteContact.ID = int.Parse(txtID.Text);


                    myConn.Open();
                    sqlDeleteCommand = "DELETE FROM Contact WHERE ID =" + deleteContact.ID + ";";
                    SqlCommand myDeleteCommand = new SqlCommand(sqlDeleteCommand, myConn);
                    myDeleteCommand.ExecuteNonQuery();
                    myConn.Close();


                }
                MessageBox.Show("Contact removed successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //SELECTS FROM TABLE AND USE LINQ

            //****************THERE IS NOT ANYONE WITH LAST NAME SMITH, I TESTED WITH DIFFERENT LASTNAMES **************

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            try
            {


                lstOutput.Items.Clear();
                   using (SqlConnection myConnection = new SqlConnection(connectStr))
                    {
                        string sqlCommand;
                        myConnection.Open();
                        sqlCommand = "SELECT * FROM Contact";
                        SqlCommand myCommand = new SqlCommand(sqlCommand, myConnection);
                        SqlDataReader myReader = myCommand.ExecuteReader(); ;

                        while (myReader.Read())
                        {
                            Contact tempContact = new Contact();
                            tempContact.ID = int.Parse(myReader["ID"].ToString());
                            tempContact.FirstName = myReader["FirstName"].ToString();
                            tempContact.LastName = myReader["LastName"].ToString();
                            tempContact.Phone = myReader["Phone"].ToString();
                            tempContact.Email = myReader["Email"].ToString();

                        listContacts.Add(tempContact);

                    }
                        myReader.Close();
                        myConnection.Close();
                    MessageBox.Show("Successful");
                    
                    int k;
                    Contact tempCont = new Contact();

                    List<Contact> contactFilter = new List<Contact>();


                    var query = from contactF in listContacts
                                where contactF.LastName == "Smith"
                                select contactF;
                    contactFilter = query.ToList<Contact>();


                    for (k = 0; k < contactFilter.Count; k++)
                    {
                        tempCont = contactFilter[k];
                        lstOutput.Items.Add("ID = " + tempCont.ID.ToString() + " FirstName = " + tempCont.FirstName + " LastName = " + tempCont.LastName +" Phone = "+tempCont.Phone+ " Email = " + tempCont.Email);
                    }
                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECTS DATA AND SAVE IT INTO A TEXT FILE IN C:\

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection myConnection = new SqlConnection(connectStr))
                {
                    string sqlCommand;
                    myConnection.Open();
                    sqlCommand = "SELECT * FROM Contact";
                    SqlCommand myCommand = new SqlCommand(sqlCommand, myConnection);
                    SqlDataReader myReader = myCommand.ExecuteReader(); ;

                    while (myReader.Read())
                    {
                        Contact tempContact = new Contact();
                        tempContact.ID = int.Parse(myReader["ID"].ToString());
                        tempContact.FirstName = myReader["FirstName"].ToString();
                        tempContact.LastName = myReader["LastName"].ToString();
                        tempContact.Phone = myReader["Phone"].ToString();
                        tempContact.Email = myReader["Email"].ToString();

                        listContacts.Add(tempContact);

                    }
                    myReader.Close();
                    myConnection.Close();
                }

                string pathName = @"C:\";

                string fileName = @"DatabaseBackup_2020-02-14.txt";
                int k;
                StreamWriter sw = new StreamWriter(Path.Combine(pathName, fileName), false);

                for (k = 0; k < listContacts.Count; k++)
                {
                    Contact tempContact = new Contact();

                    tempContact = listContacts[k];
                    sw.WriteLine("ID = " + tempContact.ID.ToString() + " FirstName = " + tempContact.FirstName + " LastName = " + tempContact.LastName + " Phone = " + tempContact.Phone + " Email = " + tempContact.Email);

                }
                sw.Close();
                MessageBox.Show("Contacts saved!");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //READS INFO FROM THE TEXT FILE 

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string pathName = @"C:\Andres";

                string fileName = @"DatabaseBackup_2020-02-14.txt";

                string pfn = Path.Combine(pathName, fileName);

                if (File.Exists(pfn) == true)
                {
                    StreamReader sr = new StreamReader(pfn);
                    while (true)
                    {
                        string line = sr.ReadLine();
                        if (line == null) break;
                        lstOutput.Items.Add(line);

                    }
                    sr.Close();
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CLASS CONTACT CREATED IN A DIFERENT FILE Contact.cs 
    }
}
