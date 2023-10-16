﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AppsGestionStock
{
    public partial class Form1 : Form
    {
        WcfService.Service1Client service = new WcfService.Service1Client();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgProduit.DataSource = service.getProduits();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            WcfService.Produit p = new WcfService.Produit();
          
            p.Description = txtDescription.Text;
            p.Libelle = txtLibelle.Text;
            p.Pu= !string.IsNullOrEmpty(txtPrixUnitaire.Text) ? double.Parse(txtPrixUnitaire.Text) : 0;
            p.Qte = !string.IsNullOrEmpty(txtQuantite.Text) ? double.Parse(txtQuantite.Text) : 0;
            service.AjouterProduit(p);
            effacer();

        }
        private void effacer()
        {
            txtQuantite.Text = string.Empty;
            txtDescription.Text = string.Empty; 
            txtId.Text = string.Empty;
            txtLibelle.Text = string.Empty;
            txtPrixUnitaire.Text = string.Empty;
            dgProduit.DataSource = service.getProduits();
            txtLibelle.Focus();
        }

        private void btnChoisir_Click(object sender, EventArgs e)
        {
            int? id = int.Parse(dgProduit.CurrentRow.Cells[4].Value.ToString());
            var p = service.getProduit(id);
            txtId.Text = p.idProduit.ToString();
            txtQuantite.Text = p.Qte.ToString();
            txtDescription.Text = p.Description;
            txtPrixUnitaire.Text = p.Pu.ToString();
            txtLibelle.Text = p.Libelle;


        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            WcfService.Produit p = new WcfService.Produit();
            p.idProduit = !string.IsNullOrEmpty(txtId.Text) ? int.Parse(txtId.Text) : 0;
            p.Description = txtDescription.Text;
            p.Libelle = txtLibelle.Text;
            p.Pu = !string.IsNullOrEmpty(txtPrixUnitaire.Text) ? double.Parse(txtPrixUnitaire.Text) : 0;
            p.Qte = !string.IsNullOrEmpty(txtQuantite.Text) ? double.Parse(txtQuantite.Text) : 0;
            service.ModifierProduit(p);
            effacer();
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            int? id = int.Parse(txtId.Text);
            var p = service.SupprimerProduit(id);
            effacer();
        }

        private void btnRechercher_Click(object sender, EventArgs e)
        {
            dgProduit.DataSource = service.RechercherProduit(txtLibelle.Text, txtDescription.Text);
        }
    }
}
