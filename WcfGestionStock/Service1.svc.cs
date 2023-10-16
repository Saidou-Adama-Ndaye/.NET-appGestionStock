using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WcfGestionStock.Model;
using System.Data.Entity;

namespace WcfGestionStock
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class Service1 : IService1
    {
        
        private BdStockEntities db = new BdStockEntities();
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="composite"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool AjouterProduit(Produit p)
        {
            try
            {
                db.Produit.Add(p);
                db.SaveChanges();
                return true;

            }catch (Exception ex) { }
            return false;
        }

        public List<Produit> getProduits()
        {
            return db.Produit.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Produit getProduit(int? id) 
        {
            return db.Produit.Find(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool ModifierProduit(Produit p)
        {
            try
            {
                db.Entry(p).State=EntityState.Modified;
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            { }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SupprimerProduit(int?  id)
        {
            try
            {
                var p = db.Produit.Find(id);
                db.Produit.Remove(p);
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            { }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public List<Produit> RechercherProduit(string Libelle, string Description)
        {
            var liste = db.Produit.ToList();

            if (!string.IsNullOrEmpty(Libelle))
            {
                liste = liste.Where(s => s.Libelle != null).ToList();
                liste = liste.Where(s => s.Libelle.ToUpper().Contains(Libelle.ToUpper())).ToList();

            }

            if (!string.IsNullOrEmpty(Description))
            {
                liste = liste.Where(s => s.Description != null).ToList();
                liste = liste.Where(s => s.Description.ToUpper().Contains(Description.ToUpper())).ToList();

            }

            // if (Qte != 0)
            //{
            //    liste = liste.Where(s => s.Qte != null).ToList();
            //   liste = liste.Where(s => s.Qte == Qte).ToList();

            // }

            //if (PU !=0)
            //{
            //  liste = liste.Where(s => s.PU != null).ToList();
            //  liste = liste.Where(s => s.PU == PU).ToList();

            // }

            return liste.ToList();
        } 
    }
}
