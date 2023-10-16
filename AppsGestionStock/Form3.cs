using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using System.Configuration;

namespace AppsGestionStock
{
    public partial class Form3 : Form
    {
        // HttpClient pour effectuer les requêtes HTTP vers l'API
        private HttpClient client;
        // URL de base de l'API de gestion de stock
        private string apiBaseUrl;

        public Form3()
        {
            InitializeComponent();

            // Lire l'URL de l'API depuis le fichier App.config
           apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            // Initialisation du HttpClient avec l'URL de base de l'API
            client = new HttpClient();
            client.BaseAddress = new Uri(apiBaseUrl);
        }

        // Chargement initial du formulaire
        private async void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                // Récupérer la liste des produits depuis l'API via une requête GET
                HttpResponseMessage response = await client.GetAsync("list.php");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(responseBody);

                // Afficher la liste des produits dans la DataGridView
                dgProduit.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la récupération des données des produits : " + ex.Message, "Erreur");
            }
        }

        // Gestion du clic sur le bouton "Ajouter"
        private async void btnAjouter_Click(object sender, EventArgs e)
        {
            // Récupérer les données saisies par l'utilisateur
            string Libelle = txtLibelle.Text;
            string Description = txtDescription.Text;
            double Pu = Convert.ToDouble(txtPrixUnitaire.Text);
            double Qte = Convert.ToDouble(txtQuantite.Text);

            // Créer un dictionnaire avec les données à envoyer dans la requête POST
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "Libelle", Libelle },
                { "Description", Description },
                { "Pu", Pu.ToString() },
                { "Qte", Qte.ToString() }
            };

            try
            {
                // Envoyer la requête POST à l'API pour ajouter un produit
                var content = new FormUrlEncodedContent(data);
                HttpResponseMessage response = await client.PostAsync("create.php", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                if (result.success.ToObject<bool>())
                {
                    // Afficher un message de succès
                    MessageBox.Show("Le produit a été ajouté avec succès.", "Succès");
                    // Mettre à jour la DataGridView avec les nouvelles données
                    await RefreshDataGridView();
                    // Réinitialiser les champs de saisie
                    ResetFields();
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite lors de l'ajout du produit.", "Erreur");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la communication avec l'API : " + ex.Message, "Erreur");
            }
        }

        // Gestion du clic sur le bouton "Modifier"
        private async void btnModifier_Click(object sender, EventArgs e)
        {
            // Vérifier si un produit est sélectionné dans la DataGridView
            if (dgProduit.SelectedRows.Count > 0)
            {
                // Récupérer l'ID du produit sélectionné
                int selectedProductId = Convert.ToInt32(dgProduit.SelectedRows[0].Cells["Id"].Value);
                string Libelle = txtLibelle.Text;
                string Description = txtDescription.Text;
                double Pu = Convert.ToDouble(txtPrixUnitaire.Text);
                double Qte = Convert.ToDouble(txtQuantite.Text);

                // Créer un dictionnaire avec les données à envoyer dans la requête POST
                Dictionary<string, string> data = new Dictionary<string, string>
                {
                    { "Id", selectedProductId.ToString() },
                    { "Libelle", Libelle },
                    { "Description", Description },
                    { "Pu", Pu.ToString() },
                    { "Qte", Qte.ToString() }
                };

                try
                {
                    // Envoyer la requête POST à l'API pour modifier le produit
                    var content = new FormUrlEncodedContent(data);
                    HttpResponseMessage response = await client.PostAsync("update.php", content);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                    if (result.success.ToObject<bool>())
                    {
                        // Afficher un message de succès
                        MessageBox.Show("Le produit a été modifié avec succès.", "Succès");
                        // Mettre à jour la DataGridView avec les nouvelles données
                        await RefreshDataGridView();
                        // Réinitialiser les champs de saisie
                        ResetFields();
                    }
                    else
                    {
                        MessageBox.Show("Une erreur s'est produite lors de la modification du produit.", "Erreur");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur s'est produite lors de la communication avec l'API : " + ex.Message, "Erreur");
                }
            }
        }

        // Gestion du clic sur le bouton "Supprimer"
        private async void btnSupprimer_Click(object sender, EventArgs e)
        {
            // Vérifier si un produit est sélectionné dans la DataGridView
            if (dgProduit.SelectedRows.Count > 0)
            {
                // Récupérer l'ID du produit sélectionné
                int selectedProductId = Convert.ToInt32(dgProduit.SelectedRows[0].Cells["Id"].Value);

                // Créer un dictionnaire avec l'ID du produit à supprimer
                Dictionary<string, string> data = new Dictionary<string, string>
                {
                    { "Id", selectedProductId.ToString() }
                };

                try
                {
                    // Envoyer la requête POST à l'API pour supprimer le produit
                    var content = new FormUrlEncodedContent(data);
                    HttpResponseMessage response = await client.PostAsync("delete.php", content);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                    if (result.success.ToObject<bool>())
                    {
                        // Afficher un message de succès
                        MessageBox.Show("Le produit a été supprimé avec succès.", "Succès");
                        // Mettre à jour la DataGridView avec les nouvelles données
                        await RefreshDataGridView();
                        // Réinitialiser les champs de saisie
                        ResetFields();
                    }
                    else
                    {
                        MessageBox.Show("Une erreur s'est produite lors de la suppression du produit.", "Erreur");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur s'est produite lors de la communication avec l'API : " + ex.Message, "Erreur");
                }
            }
        }

        // Gestion du clic sur le bouton "Choisir"
        private void btnChoisir_Click(object sender, EventArgs e)
        {
            // Vérifier si un produit est sélectionné dans la DataGridView
            if (dgProduit.SelectedRows.Count > 0)
            {
                // Récupérer les données du produit sélectionné
                int selectedProductId = Convert.ToInt32(dgProduit.SelectedRows[0].Cells["Id"].Value);
                string Libelle = dgProduit.SelectedRows[0].Cells["Libelle"].Value.ToString();
                string Description = dgProduit.SelectedRows[0].Cells["Description"].Value.ToString();
                double Pu = Convert.ToDouble(dgProduit.SelectedRows[0].Cells["Pu"].Value);
                double Qte = Convert.ToDouble(dgProduit.SelectedRows[0].Cells["Qte"].Value);

                // Mettre à jour les contrôles TextBox avec les données du produit sélectionné
                txtId.Text = selectedProductId.ToString();
                txtLibelle.Text = Libelle;
                txtDescription.Text = Description;
                txtPrixUnitaire.Text = Pu.ToString();
                txtQuantite.Text = Qte.ToString();
            }
        }

        // Réinitialiser les champs de saisie à leur valeur par défaut
        private void ResetFields()
        {
            txtId.Text = "";
            txtLibelle.Text = "";
            txtDescription.Text = "";
            txtPrixUnitaire.Text = "";
            txtQuantite.Text = "";
        }

        // Récupérer et afficher les données des produits dans la DataGridView
        private async Task RefreshDataGridView()
        {
            try
            {
                // Récupérer la liste des produits depuis l'API via une requête GET
                HttpResponseMessage response = await client.GetAsync("list.php");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var products = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(responseBody);

                // Afficher la liste des produits dans la DataGridView
                dgProduit.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la récupération des données des produits : " + ex.Message, "Erreur");
            }
        }
    }

    // Classe représentant un produit
    public class Product
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public double Pu { get; set; }
        public double Qte { get; set; }
    }
}
