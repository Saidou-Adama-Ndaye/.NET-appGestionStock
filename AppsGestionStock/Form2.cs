using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebApiStock.Models;

namespace AppsGestionStock
{
    public partial class Form2 : Form
    {
        private const string BaseUrl = "https://localhost:44357/";

        public Form2()
        {
            InitializeComponent();
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            await LoadProduits();
        }

        private async Task LoadProduits()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("apigl/Produits/GetProduits");
                if (response.IsSuccessStatusCode)
                {
                    var produits = await response.Content.ReadAsAsync<IEnumerable<Produit>>();
                    dgProduit.DataSource = produits.ToList();
                }
                else
                {
                    MessageBox.Show("Erreur lors du chargement des produits.");
                }
            }
        }

        private async Task<Produit> GetProduit(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"apigl/Produits/GetProduit/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var produit = await response.Content.ReadAsAsync<Produit>();
                    return produit;
                }

                return null;
            }
        }

        private async Task<int?> AddProduit(Produit produit)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("apigl/Produits/PostProduit", produit);
                if (response.IsSuccessStatusCode)
                {
                    var id = await response.Content.ReadAsAsync<int?>();
                    return id;
                }

                return null;
            }
        }

        private async Task<bool> UpdateProduit(int id, Produit produit)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PutAsJsonAsync($"apigl/Produits/PutProduit/{id}", produit);
                return response.IsSuccessStatusCode;
            }
        }

        private async Task<bool> DeleteProduit(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"apigl/Produits/DeleteProduit/{id}");
                return response.IsSuccessStatusCode;
            }
        }

        private async void btnAjouter_Click(object sender, EventArgs e)
        {
            Produit nouveauProduit = new Produit
            {
                DescriptionProduit = txtDescription.Text,
                LibelleProduit = txtLibelle.Text,
                QteProduit = float.Parse(txtQuantite.Text),
                PuProduit = float.Parse(txtPrixUnitaire.Text)
            };

            int? idProduit = await AddProduit(nouveauProduit);
            if (idProduit.HasValue)
            {
                await LoadProduits();
                MessageBox.Show("Produit ajouté avec succès.");
            }
            else
            {
                MessageBox.Show("Erreur lors de l'ajout du produit.");
            }
        }

        private async void btnModifier_Click(object sender, EventArgs e)
        {
            if (dgProduit.SelectedRows.Count > 0)
            {
                int idProduit = (int)dgProduit.SelectedRows[0].Cells["IdProduit"].Value;

                Produit produitAModifier = await GetProduit(idProduit);
                if (produitAModifier != null)
                {
                    produitAModifier.LibelleProduit = txtLibelle.Text;
                    produitAModifier.DescriptionProduit = txtDescription.Text;
                    produitAModifier.QteProduit = float.Parse(txtQuantite.Text);
                    produitAModifier.PuProduit = float.Parse(txtPrixUnitaire.Text);

                    bool success = await UpdateProduit(idProduit, produitAModifier);
                    if (success)
                    {
                        await LoadProduits();
                        MessageBox.Show("Produit modifié avec succès.");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification du produit.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un produit à modifier.");
            }
        }

        private async void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (dgProduit.SelectedRows.Count > 0)
            {
                int idProduit = (int)dgProduit.SelectedRows[0].Cells["IdProduit"].Value;

                bool success = await DeleteProduit(idProduit);
                if (success)
                {
                    await LoadProduits();
                    MessageBox.Show("Produit supprimé avec succès.");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression du produit.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un produit à supprimer.");
            }
        }

        private async void btnChoisir_Click(object sender, EventArgs e)
        {
            if (dgProduit.SelectedRows.Count > 0)
            {
                int idProduit = (int)dgProduit.SelectedRows[0].Cells["IdProduit"].Value;

                Produit produitChoisi = await GetProduit(idProduit);
                if (produitChoisi != null)
                {
                    txtLibelle.Text = produitChoisi.LibelleProduit;
                    txtDescription.Text = produitChoisi.DescriptionProduit;
                    txtQuantite.Text = produitChoisi.QteProduit.ToString();
                    txtPrixUnitaire.Text = produitChoisi.PuProduit.ToString();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un produit.");
            }
        }
    }
}
