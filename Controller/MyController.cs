using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
using Projeto_Final_P2.Model;
using System.Windows.Data;
using System.Windows;
using Projeto_Final_P2.Views;
using System.Data.Entity.Validation;

namespace Projeto_Final_P2.Controller
{
    public class MyController : UIElement
    {
        MainWindow main;
        public Cmd cmdNavegar { get; set; }
        public Cmd cmdInserirUtilizador { get; set; }

        public Cmd cmdApagarUtilizador { get; set; }

        public Cmd cmdGravarUtilizador { get; set; }
        public MyController() {

            main = (MainWindow)App.Current.MainWindow;
            cmdNavegar = new Cmd(navega, (parametro) => true);
            cmdInserirUtilizador = new Cmd(inserirUtilizador, (parametro) => true);
            cmdApagarUtilizador = new Cmd(apagarUtilizador, (parametrro) => true);
            cmdGravarUtilizador = new Cmd(gravarUtilizador, (parametro) => true);
            iniciaUtilizadores();
        }
        #region Navegar

        //public Boolean podeNavegar(object pagina)
        //{
        //    string pag = pagina.ToString();
        //    var paginaCorrente = main.myFrame.Content as Page;
        //    if (pag == paginaCorrente.Title) return false;
        //    return true;
        //}


        public void navega(Object pagina){
            string pag = pagina.ToString();
            switch (pag)
            {
                case "App_Page":
                    App_Page p = new App_Page();
                    main.myFrame.Navigate(p);
                    break;
                case "Home_Page":
                    Home_Page h = new Home_Page();
                    main.myFrame.Navigate(h);
                    break;
            }
        }

        #endregion

        #region Utilizadores



        public ObservableCollection<utilizadores> Utilizadores
        {
            get { return (ObservableCollection<utilizadores>)GetValue(UtilizadoresProperty); }
            set { SetValue(UtilizadoresProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Utilizadores.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UtilizadoresProperty =
            DependencyProperty.Register("Utilizadores",
                typeof(ObservableCollection<utilizadores>),
                typeof(MyController), new PropertyMetadata(null));





        public ICollectionView ViewUtilizadores
        {
            get { return (ICollectionView)GetValue(ViewUtilizadoresProperty); }
            set { SetValue(ViewUtilizadoresProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewUtilizadores.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewUtilizadoresProperty =
            DependencyProperty.Register("ViewUtilizadores", typeof(ICollectionView), typeof(MyController), new PropertyMetadata(null));



        public utilizadores UtilizadorCorrente
        {
            get { return (utilizadores)GetValue(UtilizadorCorrenteProperty); }
            set { SetValue(UtilizadorCorrenteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UtilizadorCorrente.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UtilizadorCorrenteProperty =
            DependencyProperty.Register("UtilizadorCorrente", typeof(utilizadores), typeof(MyController), new PropertyMetadata(null));





        public void iniciaUtilizadores() {

            using (P2_Project_Data_BaseEntities db = new P2_Project_Data_BaseEntities())
            {
                List<utilizadores> utilizadores = db.utilizadores.ToList();
                Utilizadores = new ObservableCollection<utilizadores>(utilizadores);
                ViewUtilizadores = CollectionViewSource.GetDefaultView(Utilizadores);
                ViewUtilizadores.CurrentChanged += ViewUtilizadores_CurrentChanged;
                UtilizadorCorrente = (utilizadores)ViewUtilizadores.CurrentItem;


            }
        }

        private void ViewUtilizadores_CurrentChanged(object sender, EventArgs e)
        {
            UtilizadorCorrente = ViewUtilizadores.CurrentItem as utilizadores;
        }

        public void inserirUtilizador(object parametro)
        {
            using (P2_Project_Data_BaseEntities db = new P2_Project_Data_BaseEntities()) {
                try
                {
                    
                    //utilizadores nova = new utilizadores();
                    //db.utilizadores.Add(nova);
                    //db.SaveChanges();
                    //iniciaUtilizadores();
                    //ViewUtilizadores.MoveCurrentToLast();


                    utilizadores nova = new utilizadores();

                    App_Page p2 = main.myFrame.Content as App_Page;
                    nova.id_utilizador = nova.id_utilizador += 1;
                    nova.nome = p2.registoNome.Text;
                    nova.data_nascimento = DateTime.Parse(p2.registoDataNascimento.Text);
                    nova.morada = p2.registoMorada.Text;
                    nova.numero_predio = Int32.Parse(p2.registoNumeroPredio.Text);
                    nova.numero_andar = Int32.Parse(p2.registoNumeroAndar.Text);
                    nova.cc = Int32.Parse(p2.registoCc.Text);
                    db.utilizadores.Add(nova);

                    p2.registoNome.Text = "";
                    p2.registoDataNascimento.Text = "";
                    p2.registoMorada.Text = "";
                    p2.registoNumeroPredio.Text = "";
                    p2.registoNumeroAndar.Text = "";
                    p2.registoCc.Text = "";


                    db.SaveChanges();
                    iniciaUtilizadores();
                    ViewUtilizadores.MoveCurrentToLast();

                }
                catch (DbEntityValidationException a)
                {
                    Console.WriteLine(a);
                }
                
            }
        }


        public void apagarUtilizador(object parametro)
        {

            using (P2_Project_Data_BaseEntities db = new P2_Project_Data_BaseEntities())
            {
                int id;
                if(int.TryParse(parametro.ToString(), out id)){
                    utilizadores eliminado = db.utilizadores.Find(id);
                    if(eliminado != null) db.utilizadores.Remove(eliminado);
                    db.SaveChanges();
                    iniciaUtilizadores();
                    ViewUtilizadores.MoveCurrentToLast();
                }
                
            }
        }



        public void gravarUtilizador(object parametro)
        {
            using (P2_Project_Data_BaseEntities db = new P2_Project_Data_BaseEntities())
            {
                utilizadores cur = ViewUtilizadores.CurrentItem as utilizadores;
                utilizadores esta = db.utilizadores.Find(cur.id_utilizador);
                if(esta != null)
                {
                    App_Page p = main.myFrame.Content as App_Page;
                    esta.nome = p.txtnome.Text;
                    esta.data_nascimento = DateTime.Parse(p.txtdatanasc.Text);
                    esta.idade = Int32.Parse(p.txtidade.Text);
                    esta.morada = p.txtmorada.Text;
                    esta.numero_predio = Int32.Parse(p.txtnpre.Text);
                    esta.numero_andar = Int32.Parse(p.txtnand.Text);
                    esta.cc = Int32.Parse(p.txtcc.Text);

                }
                db.SaveChanges();
                iniciaUtilizadores();
            }
                
        }





        #endregion

    }
}
