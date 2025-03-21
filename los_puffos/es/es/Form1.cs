using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace es
{
    public partial class Form1 : Form
    {
        int punti_puffo = 0;
        int punti_garga = 0;
        bool turno_puffo = true;
        public Random rnd;
        Panel[] alberi;

        public Form1()
        {
            InitializeComponent();
            rnd = new Random();
            dimensioni_campo();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            alberi = new Panel[] { pnl_albero1, pnl_albero2, pnl_albero3, pnl_albero4, pnl_albero5, pnl_albero6, pnl_albero7, pnl_albero8, pnl_albero9, pnl_albero10 };
            crea(pnl_casa, coord_a_caso(pnl_casa.Width), coord_a_caso(pnl_casa.Height));
            crea(pnl_puffo, coord_a_caso(pnl_puffo.Width), coord_a_caso(pnl_puffo.Height));
            crea(pnl_garga, coord_a_caso(pnl_garga.Width), coord_a_caso(pnl_garga.Height));
            crea_alberi(alberi);
            aggiorna(pnl_casa, coord_a_caso(pnl_casa.Width), coord_a_caso(pnl_casa.Height));
            aggiorna(pnl_puffo, coord_a_caso(pnl_puffo.Width), coord_a_caso(pnl_puffo.Height));
            aggiorna(pnl_garga, coord_a_caso(pnl_garga.Width), coord_a_caso(pnl_garga.Height));
            lbl_garga_punti.Text = $"Punti Gargamella: {punti_garga}";
            lbl_puffo_punti.Text = $"Punti Puffo: {punti_puffo}";
        }

        public void crea_alberi(Panel[] cosa)
        {
            for (int i = 0; i < cosa.Length; i++)
            {
                int x = coord_a_caso(cosa[i].Width);
                int y = coord_a_caso(cosa[i].Height);
                cosa[i].Location = new Point(x, y);
            }
        }

        public void crea(Panel cosa, int x, int y)
        {
            cosa.Location = new Point(x, y);
        }

        private void dimensioni_campo()
        {
            pnl_casa.Width = 30;
            pnl_casa.Height = 30;
            pnl_puffo.Width = 30;
            pnl_puffo.Height = 30;
            pnl_garga.Width = 30;
            pnl_garga.Height = 30;
            pnl_campo.Width = 390;
            pnl_campo.Height = 390;
        }

        private int coord_a_caso(int min)
        {
            int massimo = pnl_campo.Width - min;
            return rnd.Next(0, massimo);
        }

        private void aggiorna(Panel cosa, int x, int y)
        {
            int x_min = 0;
            int y_min = 0;
            int x_max = pnl_campo.Width - cosa.Width;
            int y_max = pnl_campo.Height - cosa.Height;
            int x_try = cosa.Location.X + x;
            int y_try = cosa.Location.Y + y;

            if (x_try >= x_min && x_try <= x_max && y_try >= y_min && y_try <= y_max)
            {
                cosa.Location = new Point(x_try, y_try);
                controlla(cosa);
                controlla_albero(cosa);
            }
        }

        private void controlla(Panel panel)
        {
            
            if (panel == pnl_puffo && panel.Bounds.IntersectsWith(pnl_casa.Bounds))
            {
                if (turno_puffo)
                {
                    punti_puffo += 10;
                    lbl_puffo_punti.Text = $"Punti Puffo: {punti_puffo}";
                }

                aggiorna(pnl_casa, coord_a_caso(pnl_casa.Width), coord_a_caso(pnl_casa.Height));
                crea_alberi(alberi);
            }

            if (panel == pnl_garga && panel.Bounds.IntersectsWith(pnl_puffo.Bounds))
            {
                punti_garga += 10;
                lbl_garga_punti.Text = $"Punti Gargamella: {punti_garga}";
                aggiorna(pnl_casa, coord_a_caso(pnl_casa.Width), coord_a_caso(pnl_casa.Height));
                crea_alberi(alberi);
            }
        }

        private void controlla_albero(Panel panel)
        {
            foreach (var albero in alberi)
            {
                if (panel.Bounds.IntersectsWith(albero.Bounds))
                {
                    if(panel == pnl_puffo)
                    {
                        punti_puffo -= 5;
                        lbl_puffo_punti.Text = $"Punti Puffo: {punti_puffo}";
                    } else
                    {
                        punti_garga -= 5;
                        lbl_garga_punti.Text = $"Punti Gargamella: {punti_garga}";
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int x = 0, y = 0;

            if (e.KeyCode == Keys.W) y = -10;
            if (e.KeyCode == Keys.A) x = -10;
            if (e.KeyCode == Keys.S) y = 10;
            if (e.KeyCode == Keys.D) x = 10;

            if (turno_puffo)
            {
                aggiorna(pnl_puffo, x, y);
                turno_puffo = false;
            }
            else
            {
                aggiorna(pnl_garga, x, y);
                turno_puffo = true;
            }
        }

        private void muovi(int x, int y)
        {
            if (turno_puffo)
            {
                aggiorna(pnl_puffo, x, y);
                turno_puffo = false;
            }
            else
            {
                aggiorna(pnl_garga, x, y);
                turno_puffo = true;
            }
        }

        private void btn_puffo_up_Click(object sender, EventArgs e)
        {
            if (turno_puffo)
            {
                muovi(0, -10);
            }
        }

        private void btn_puffo_down_Click(object sender, EventArgs e)
        {
            if (turno_puffo)
            {
                muovi(0, 10);
            }
        }

        private void btn_puffo_right_Click(object sender, EventArgs e)
        {
            if (turno_puffo)
            {
                muovi(10, 0);
            }
        }

        private void btn_puffo_left_Click(object sender, EventArgs e)
        {
            if (turno_puffo)
            {
                muovi(-10, 0);
            }
        }

        private void btn_garga_up_Click(object sender, EventArgs e)
        {
            if (!turno_puffo)
            {
                muovi(0, -10);
            }
        }

        private void btn_garga_down_Click(object sender, EventArgs e)
        {
            if (!turno_puffo)
            {
                muovi(0, 10);
            }
        }

        private void btn_garga_right_Click(object sender, EventArgs e)
        {
            if (!turno_puffo)
            {
                muovi(10, 0);
            }
        }

        private void btn_garga_left_Click(object sender, EventArgs e)
        {
            if (!turno_puffo)
            {
                muovi(-10, 0);
            }
        }
    }
}
