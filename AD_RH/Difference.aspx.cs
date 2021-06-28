using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ADODB;
using System.Data;
using System.DirectoryServices;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.IO;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
namespace AD_RH
{
    public class ADO
    {
        private string Login;
        private string Nom;
        private string Prenom;
        private string Email;
        private string Tel;
        private string Description;

        public void setLogin(string n)
        {
            this.Login = n;
        }
        public string getLogin()
        {
            return this.Login;
        }
        public void setNom(string n)
        {
            this.Nom = n;
        }
        public string getNom()
        {
            return this.Nom;
        }
        public void setPrenom(string n)
        {
            this.Prenom = n;
        }
        public string getPrenom()
        {
            return this.Prenom;
        }
        public void setEmail(string n)
        {
            this.Email = n;
        }
        public string getEmail()
        {
            return this.Email;
        }
        public void setTel(string n)
        {
            this.Tel = n;
        }
        public string getTel()
        {
            return this.Tel;
        }
        public void setDescription(string n)
        {
            this.Description = n;
        }
        public string getDescription()
        {
            return this.Description;
        }
        public ADO(ADO a)
        {
            Login = a.getLogin();
            Nom = a.getNom();
            Prenom = a.getPrenom();
            Email = a.getEmail();
            Tel = a.getTel();
            Description = a.getDescription();
        }
        public ADO()
        {
            Login = "";
            Nom = "";
            Prenom = "";
            Email = "";
            Tel = "";
            Description = "";
        }
        public ADO(string l, string n, string p, string e, string t, string d)
        {
            Login = l;
            Nom = n;
            Prenom = p;
            Email = e;
            Tel = t;
            Description = d;
        }

    }
    public partial class WebForm1 : System.Web.UI.Page
    {
        public List<ADO> rhdatabase()
        {
            string usercoderh, nomrh, prenomrh;
            List<ADO> rh = new List<ADO>();
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings["SQL_RHDB"]);
            string insertSQL = "SELECT usercode, Nom, Prenom FROM `user` WHERE (Actif = 0) AND (usercode <> '') ORDER BY usercode";
            connection.Open();
            MySqlCommand DbCommand = new MySqlCommand(insertSQL, connection);
            DbCommand.CommandText = insertSQL;
            MySqlDataReader reader = DbCommand.ExecuteReader();
            int i = 0;
            // parcours resultat requete sur rh
            var watch2 = System.Diagnostics.Stopwatch.StartNew();
            while (reader.Read())
            {
                i++;
                // set usercode, nom et prenom depuis la base rh en miniscule sans espace
                usercoderh = reader[0].ToString().Trim().ToLower();
                nomrh = reader[1].ToString().Trim().ToLower();
                prenomrh = reader[2].ToString().Trim().ToLower();
                rh.Add(new ADO(usercoderh, nomrh, prenomrh, "", "", ""));
            }
            connection.Close();
            return rh;
        }//liste utilisateur desactivé dans la base rh
        public void writetitaniumgrid(List<ADO> ad, MySqlConnection connection)
        {
            DataTable dt = new DataTable();
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Service", typeof(string));
                dt.Columns.Add("Nom", typeof(string));
                dt.Columns.Add("Prenom", typeof(string));
            }
            DataRow NewRow;
            List<ADO> rh = rhdatabase();
            foreach (ADO myObj in rh)
            {
                Thread myThread = new Thread(() =>
                {
                    ADO resc = findalltitanium(myObj, ad, connection);
                    if (resc.getNom() != "")
                    {
                        NewRow = dt.NewRow();
                        NewRow[0] = resc.getDescription();
                        NewRow[1] = resc.getNom();
                        NewRow[2] = resc.getPrenom();
                        dt.Rows.Add(NewRow);
                    }
                });
                myThread.Start();
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }//rempli grid ==> appel findalltitanium
        public void writegrid(List<ADO> ad, MySqlConnection connection, bool b)
        {
            DataTable dt = new DataTable();
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("usercode", typeof(string));
                dt.Columns.Add("Nom", typeof(string));
                dt.Columns.Add("Prenom", typeof(string));
                if (b == true) dt.Columns.Add("Date Expiration (AD)", typeof(string));
            }
            DataRow NewRow;
            List<ADO> rh = rhdatabase();
            foreach (ADO myObj in rh)
            {
                Thread myThread = new Thread(() =>
                {
                    ADO resc = findall(myObj, ad, connection, b);
                    if (resc.getLogin() != "")
                    {
                        NewRow = dt.NewRow();
                        NewRow[0] = resc.getLogin();
                        NewRow[1] = resc.getNom();
                        NewRow[2] = resc.getPrenom();
                        if (b == true) NewRow[3] = resc.getDescription();
                        dt.Rows.Add(NewRow);
                    }

                });
                myThread.Start();
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }//rempli grid ==> appel findall
        public void writegridCompta(List<ADO> ad, MySqlConnection connection)
        {
            DataTable dt = new DataTable();
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("usercode", typeof(string));
                dt.Columns.Add("Nom", typeof(string));
                dt.Columns.Add("Prenom", typeof(string));
            }
            DataRow NewRow;
            List<ADO> rh = rhdatabase();
            foreach (ADO myObj in rh)
            {
                Thread myThread = new Thread(() =>
                {
                    ADO resc = findAllCompta(myObj, ad, connection);
                    if (resc.getLogin() != "")
                    {
                        NewRow = dt.NewRow();
                        NewRow[0] = resc.getLogin();
                        NewRow[1] = resc.getNom();
                        NewRow[2] = resc.getPrenom();
                        dt.Rows.Add(NewRow);
                    }
                });
                myThread.Start();
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("try connected");
            Label1.Text = "";
            GridView1.Visible = true;
            Label2.Visible = false;
            Label3.Visible = false;
            Button5.Visible = false;
            FileUpload1.Visible = false;
            DropDownList1.Visible = false; Label4.Visible = false;

        }
        public ADO findAllCompta(ADO user, List<ADO> listado, MySqlConnection connection)
        {
            //DataColumn dc = new DataColumn();
            string usercode, nom, prenom, insertSQL;
            int nbold = 0;
            List<ADO> rh = new List<ADO>();
            MySqlCommand DbCommand;
            DbCommand = connection.CreateCommand();
            foreach (ADO a in listado)
            {
                usercode = a.getLogin().Trim().ToLower();
                nom = a.getNom().Trim().ToLower();
                prenom = a.getPrenom().Trim().ToLower();
                ADO inactif;
                //meme usercode
                if (usercode == user.getLogin().Trim().ToLower())
                {
                    inactif = user;
                    inactif.setLogin(a.getLogin());
                    return inactif;
                }
                // dans le cas ou le compte est ancien "_old" verification de l'inexistance d'un compte actif pour la meme personne
                else
                {
                    if ((user.getLogin().Trim().ToLower()) == (usercode + "_old"))
                    {
                        insertSQL = "SELECT count(*) FROM `user` WHERE (Actif = 1) and usercode='" + a.getLogin() + "'";
                        try
                        {
                            DbCommand.CommandText = insertSQL;
                            nbold = (int)DbCommand.ExecuteScalar();
                            if (nbold == 0)
                            {
                                inactif = user;
                                inactif.setLogin(a.getLogin());
                                return inactif;
                            }
                        }
                        catch (Exception e) { return new ADO(); }
                    }
                }
            }
            return new ADO();
        }
        private ADO findall(ADO user, List<ADO> listado, MySqlConnection connection, bool b)
        {
            //DataColumn dc = new DataColumn();
            string usercode, nom, prenom, insertSQL;
            int nbold = 0;
            List<ADO> rh = new List<ADO>();
            MySqlCommand DbCommand;
            DbCommand = connection.CreateCommand();
            foreach (ADO a in listado)
            {
                usercode = a.getLogin().Trim().ToLower();
                nom = a.getNom().Trim().ToLower();
                prenom = a.getPrenom().Trim().ToLower();
                // meme nom et prenom
                if (nom == user.getNom().Trim().ToLower() && prenom == user.getPrenom().Trim().ToLower())
                {
                    //meme usercode
                    if (usercode == user.getLogin().Trim().ToLower())
                    {
                        return a;
                    }
                    // dans le cas ou le compte est ancien "_old" verification de l'inexistance d'un compte actif pour la meme personne
                    else
                    {
                        if ((user.getLogin().Trim().ToLower()) == (usercode + "_old"))
                        {
                            insertSQL = "SELECT count(*) FROM `user` WHERE (Actif = 1) and usercode='" + a.getLogin() + "'";
                            try
                            {
                                DbCommand.CommandText = insertSQL;
                                nbold = (int)DbCommand.ExecuteScalar();
                                if (nbold == 0)
                                {
                                    return a;
                                }
                            }
                            catch (Exception e) { return new ADO(); }

                        }
                    }
                }
            }
            return new ADO();
        }//genere le rapport pour AD,Millenium et avanteam ==> traité par usercode
        private ADO findalltitanium(ADO user, List<ADO> listado, MySqlConnection connection)
        {
            //DataColumn dc = new DataColumn();
            string nom, prenom, insertSQL;
            int nbold = 0;
            MySqlDataReader reader;
            MySqlCommand DbCommand;
            DbCommand = connection.CreateCommand();
            foreach (ADO a in listado)
            {
                nom = a.getNom().Trim().ToLower();
                prenom = a.getPrenom().Trim().ToLower();
                // meme nom et prenom
                if (nom == user.getNom().Trim().ToLower() && prenom == user.getPrenom().Trim().ToLower())
                {
                    insertSQL = "SELECT count(*) FROM `user` WHERE (Actif = true) and Nom='" + a.getNom() + "' and Prenom='" + a.getPrenom() + "'";
                    try
                    {
                        DbCommand.CommandText = insertSQL;
                        nbold = (int)DbCommand.ExecuteScalar();
                        if (nbold == 0)
                        {
                            return a;
                        }
                    }
                    catch (Exception e)
                    {
                        return new ADO();
                    }
                }
            }
            return new ADO();
        }//génére le rapport pour titan ==> traité par nom et prénom
        public void cleangrid1()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();
            dt.Rows.Add(dt.NewRow());
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }// vide la grid
        private Int64 GetInt64(DirectoryEntry entry, string attr)
        {
            //we will use the marshaling behavior of
            //the searcher
            DirectorySearcher ds = new DirectorySearcher(
                entry,
                String.Format("({0}=*)", attr),
                new string[] { attr },
                SearchScope.Base
                );

            SearchResult sr = ds.FindOne();

            if (sr != null)
            {
                if (sr.Properties.Contains(attr))
                {
                    return (Int64)sr.Properties[attr][0];
                }
            }
            return -1;
        }//decodeur date expriration        
        public List<ADO> getAD(String bd)
        {
            int r;
            DateTime date;
            DateTime now = DateTime.Now;
            List<ADO> ad = new List<ADO>();
            ADO tmp;
            string l, n, p, em, t, d;
            DirectoryEntry Ldap = new DirectoryEntry(ConfigurationManager.AppSettings[bd]);
            Ldap.AuthenticationType = AuthenticationTypes.FastBind;
            DirectorySearcher searcher = new DirectorySearcher(Ldap);
            searcher.Filter = "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))";
            int i = 0;
            foreach (SearchResult result in searcher.FindAll())
            {
                // On récupère l'entrée trouvée lors de la recherche
                DirectoryEntry DirEntry = result.GetDirectoryEntry();
                //On peut maintenant afficher les informations désirées   placement dans des variable pour création de l'objet              
                l = (DirEntry.Properties["SAMAccountName"].Value == null) ? "" : DirEntry.Properties["SAMAccountName"].Value.ToString();
                n = (DirEntry.Properties["sn"].Value == null) ? "" : DirEntry.Properties["sn"].Value.ToString();
                p = (DirEntry.Properties["givenName"].Value == null) ? "" : DirEntry.Properties["givenName"].Value.ToString();
                em = (DirEntry.Properties["mail"].Value == null) ? "" : DirEntry.Properties["mail"].Value.ToString();
                t = (DirEntry.Properties["TelephoneNumber"].Value == null) ? "" : DirEntry.Properties["TelephoneNumber"].Value.ToString();
                d = (DirEntry.Properties["description"].Value == null) ? "" : DirEntry.Properties["description"].Value.ToString();

                // vérification du parametre AccountExpire on compare les comptes avec une date non saisie ou pas expiré
                try
                {
                    Int64 expire = GetInt64(DirEntry, "accountExpires");
                    // remarque sur la saisie ==> parametre non entré retourn int64 = 0 ou 9223.... 
                    // cas d autre valeur pour valeur non saisie retourné dans le catch
                    if (expire == 0 || expire == Int64.MaxValue)
                    {
                        tmp = new ADO(l, n, p, em, t, "Non renseigné");
                        ad.Add(tmp);
                    }
                    else
                    {
                        date = new DateTime(1601, 01, 01, 0, 0, 0, DateTimeKind.Utc).AddTicks(GetInt64(DirEntry, "accountExpires"));
                        r = DateTime.Compare(now, date);
                        // r = 0 ==> date egale considéré comme expiré, r>0 compte expiré
                        // r<0 compte actif
                        if (r < 0)
                        {
                            tmp = new ADO(l, n, p, em, t, date.ToString());
                            ad.Add(tmp);
                        }
                    }
                }
                catch (Exception)
                {
                    tmp = new ADO(l, n, p, em, t, "Cas inconnu");
                    ad.Add(tmp);
                    System.Diagnostics.Debug.WriteLine(l + " ----> error " + GetInt64(DirEntry, "accountExpires"));
                }
                i++;
            }
            return ad;
        } //recup AD user        
        protected void Button1_Click(object sender, EventArgs e)
        {
            DropDownList1.Visible = true;
            Label4.Visible = true;
            DropDownList1.Items.Clear();
            DropDownList1.Items.Add("Hésingue");
            DropDownList1.Items.Add("CAPDENAC");
            DropDownList1.Items.Add("Brésil");
            DropDownList1.Items.Add("Californie");
            DropDownList1.Items.Add("Chine");
            DropDownList1.Items.Add("Singapore");
            DropDownList1.Items.Add("Inde");
            DropDownList1.Items.Add("Russie");
            DropDownList1.Items.Add("UK");
            System.Diagnostics.Debug.WriteLine("try connected");
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings["SQL_RHDB"]);
            connection.Open();
            System.Diagnostics.Debug.WriteLine("connected");
            List<ADO> ad = getAD("AD");
            writegrid(ad, connection, true);
            GridView2.Visible = false;
            Label2.Visible = false;
            Label1.Text = "Différence entre les comptes actif de l'AD(Hésingue) et inactive dans la base RH";
            List<ADO> mail = new List<ADO>();
            GridView1.Visible = true;
            connection.Close();
        }//listener du 1er bouton (AD) se connecte a l'ad et génére une list d'utilisateur actif ==> fait appel a addgrid1
        protected void Button2_Click(object sender, EventArgs e)
        {
            MySqlConnection connection2 = new MySqlConnection(ConfigurationManager.AppSettings["SQL_RHDB"]);
            connection2.Open();
            List<ADO> mail = new List<ADO>();
            ADO ado;
            List<ADO> list = new List<ADO>();
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["MIL"]);
            Label1.Text = "Différence entre les comptes actif de Millenium et inactive dans la base RH";
            string insertSQL = "SELECT USERCODE, USERPRENOM, USERNOM FROM PRODUCTION.USERS where ACTIF = 'O'";
            connection.Open();
            SqlCommand command = new SqlCommand(insertSQL, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ado = new ADO(reader[0].ToString(), reader[2].ToString(), reader[1].ToString(), "", "", "");
                list.Add(ado);
            }
            writegrid(list, connection2, false);
            connection2.Close();
            connection.Close();
        }//listener du 2eme bouton (Mil) se connecte a Millenium et génére une list d'utilisateur actif ==> fait appel a addgrid1
        protected void Button3_Click(object sender, EventArgs e)
        {
            MySqlConnection connection2 = new MySqlConnection(ConfigurationManager.AppSettings["SQL_RHDB"]);
            connection2.Open();
            List<ADO> mail = new List<ADO>();
            string prenom;
            string[] usercode, nom;
            ADO ado;
            List<ADO> list = new List<ADO>();
            Label1.Text = "Différence entre les comptes actif dans Avanteam et inactive dans la base RH";
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["AVA"]);
            string insertSQL = "select value,cn_name, lock_date, unlock_date "
                + "from DirectoryResourceLocks drl "
                + "inner join DirectoryResources dr on drl.resource = dr.cn_name "
                + "inner join DirectoryResourceAttributes dra on dr.id = dra.id_resource "
                + "where drl.locked = 0 and dra.name = 'loginname' and dr.type = 'user'";
            connection.Open();
            SqlCommand command = new SqlCommand(insertSQL, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                usercode = reader[0].ToString().Split('\\');
                nom = reader[1].ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                prenom = "";
                if (usercode.Length == 2 && nom.Length > 1)
                {
                    for (int i = 1; i < nom.Length; i++)
                    {
                        prenom += nom[i];
                    }
                    ado = new ADO(usercode[1], nom[0], prenom, "", "", "");
                    list.Add(ado);
                }
            }
            writegrid(list, connection2, false);
            connection2.Close();
            connection.Close();

        }//listener du 3eme bouton (Ava) se connecte a Avanteam et génére une list d'utilisateur actif ==> fait appel a addgrid1
        protected void Button4_Click(object sender, EventArgs e)
        {
            FileUpload1.Visible = true;
            cleangrid1();
            Label3.Visible = true;
            Button5.Visible = true;
            Label1.Text = "Différence entre les comptes actif de Titanium et inactive dans la base RH";
        }// upload le fichier 
        protected void Button5_Click(object sender, EventArgs e)
        {
            //fichier dans un format précis, 1ere page header 9lignes avec 16 utilisateurs, n page header+footer 10 lignes
            //et 17 utilisateurs
            //String  prenomrh, nomrh, usercoderh, n, p;
            FileUpload1.Visible = true;
            Label3.Visible = true;
            Button5.Visible = true;
            cleangrid1();

            Label1.Text = "Différence entre les comptes actif de Titanium et inactive dans la base RH";
            using (StreamReader inputStreamReader = new StreamReader(FileUpload1.PostedFile.InputStream, Encoding.GetEncoding("iso-8859-1")))
            {
                String[] tab1, tab2;
                String line, ligne1;
                List<ADO> titanium = new List<ADO>();
                ADO tmp;
                int _ligne = 0;
                int max_ligne = 16;
                int ligneget = 0;
                int j = 1;
                // decoupage du fichier pour création de la list d'objet ADO
                while ((line = inputStreamReader.ReadLine()) != null)
                {
                    if (j <= 9)
                    {
                        j++;
                    }
                    else
                    {
                        if (ligneget == max_ligne)
                        {
                            if (max_ligne == 16) max_ligne++;
                            for (int i = 1; i < 10; i++) { line = inputStreamReader.ReadLine(); }
                            ligneget = 0;
                        }
                        else
                        {
                            ligne1 = inputStreamReader.ReadLine();
                            tab1 = line.Split(' ');
                            tab2 = ligne1.Split(' ');
                            tab1 = tab1.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                            tab2 = tab2.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                            //on garde que les actifs 
                            if (tab1.Length > 7 && tab1[0] != "Attributs:" && tab1[tab1.Length - 1] == "Actif")
                            {
                                //new ADO("", nom, prenom,"", etat, service);
                                if (tab1.Length == 8)
                                {
                                    tmp = new ADO("", tab1[1], tab1[2], "", tab1[7], tab2[0]);
                                }
                                else
                                {
                                    tmp = new ADO("", tab1[1] + " " + tab1[2], tab1[3], "", tab1[7], tab2[0]);
                                }
                                titanium.Add(tmp);
                                _ligne++;
                            }

                            ligneget++;
                        }
                    }
                }
                /*foreach (ADO a in titanium)
                {
                    System.Diagnostics.Debug.WriteLine(a.getLogin());
                }*/
                MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings["SQL_RHDB"]);
                connection.Open();
                writetitaniumgrid(titanium, connection);
                connection.Close();
            }


        }//comparaison au fichier titanium
        protected void Button6_Click(object sender, EventArgs e)
        {
            MySqlConnection connection2 = new MySqlConnection(ConfigurationManager.AppSettings["SQL_RHDB"]);
            connection2.Open();
            List<ADO> mail = new List<ADO>();
            ADO ado;
            List<ADO> list = new List<ADO>();
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["compta"]);
            Label1.Text = "Différence entre les comptes actif de Millenium Compta et inactive dans la base RH";
            string insertSQL = "SELECT USERCODE FROM COMPTA2.USERS where ACTIF = 'O'";
            connection.Open();
            SqlCommand command = new SqlCommand(insertSQL, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ado = new ADO(reader[0].ToString(), "", "", "", "", "");
                list.Add(ado);
            }
            writegridCompta(list, connection2);
            connection2.Close();
            connection.Close();

        }//listenner Buton 6 ==> Milenium Compta
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList1.Visible = true; Label4.Visible = true;
            String bd, choix;
            bd = "";
            choix = DropDownList1.SelectedValue;
            switch (choix)
            {
                case "Hésingue":
                    Label1.Text = "Différence entre les comptes actif de l'AD(Hésingue) et inactive dans la base RH";
                    bd = "AD";
                    break;
                case "CAPDENAC":
                    Label1.Text = "Différence entre les comptes actif de l'AD(Capdenac) et inactive dans la base RH";
                    bd = "CAP";
                    break;
                case "Brésil":
                    Label1.Text = "Différence entre les comptes actif de l'AD(Brésil) et inactive dans la base RH";
                    bd = "BR";
                    break;
                case "Californie":
                    Label1.Text = "Différence entre les comptes actif de l'AD(Californie) et inactive dans la base RH";
                    bd = "CA";
                    break;
                case "Chine":
                    Label1.Text = "Différence entre les comptes actif de l'AD(Chine) et inactive dans la base RH";
                    bd = "CH";
                    break;
                case "Singapore":
                    Label1.Text = "Différence entre les comptes actif de l'AD(Singapore) et inactive dans la base RH";
                    bd = "SIN";
                    break;
                case "Inde":
                    Label1.Text = "Différence entre les comptes actif de l'AD(Inde) et inactive dans la base RH";
                    bd = "IND";
                    break;
                case "Russie":
                    Label1.Text = "Différence entre les comptes actif de l'AD(Russie) et inactive dans la base RH";
                    bd = "RU";
                    break;
                case "UK":
                    Label1.Text = "Différence entre les comptes actif de l'AD(UK) et inactive dans la base RH";
                    bd = "UK";
                    break;
            }
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings["SQL_RHDB"]);
            connection.Open();
            List<ADO> ad = getAD(bd);
            writegrid(ad, connection, true);
            GridView2.Visible = false;
            Label2.Visible = false;
            List<ADO> mail = new List<ADO>();
            GridView1.Visible = true;
            connection.Close();
        } // listner liste BC de l'AD (initialisé Bouton 1)
    }
}