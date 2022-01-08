using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class Persone
    {
        public List<Persona> persone;
        public Persone()
        {
            //inizializzazione lista
            this.persone = new List<Persona>();
            CaricaPersone();

        }

        public void Aggiungi(Persona p)
        {
            persone.Add(p);
        }
        public void Delete(int pos)
        {
            persone.RemoveAt(pos - 1); //elimina e ricompatta tutto di default
        }
        public Persona getPersonaggio(int i)
        {
            return persone.ElementAt(i);
        }
        public int nEl()
        {
            return persone.Count();//restituisce quanti elementi sono presenti
        }
        public void CaricaPersone()
        {
            Persona tmp = new Persona("alessandro", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\alessandro.png");
            Aggiungi(tmp);
            tmp = new Persona("alfredo", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\alfredo.png");
            Aggiungi(tmp);
            tmp = new Persona("anita", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\anita.png");
            Aggiungi(tmp);
            tmp = new Persona("anna", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\anna.png");
            Aggiungi(tmp);
            tmp = new Persona("bernardo", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\bernardo.png");
            Aggiungi(tmp);
            tmp = new Persona("carlo", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\carlo.png");
            Aggiungi(tmp);


            tmp = new Persona("chiara", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\chiara.png");
            Aggiungi(tmp);
            tmp = new Persona("davide", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\davide.png");
            Aggiungi(tmp);
            tmp = new Persona("ernesto", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\ernesto.png");
            Aggiungi(tmp);
            tmp = new Persona("filippo", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\filippo.png");
            Aggiungi(tmp);
            tmp = new Persona("giacomo", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\giacomo.png");
            Aggiungi(tmp);
            tmp = new Persona("giorgio", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\giorgio.png");
            Aggiungi(tmp);

            tmp = new Persona("giuseppe", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\giuseppe.png");
            Aggiungi(tmp);
            tmp = new Persona("guglielmo", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\guglielmo.png");
            Aggiungi(tmp);
            tmp = new Persona("manuele", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\manuele.png");
            Aggiungi(tmp);
            tmp = new Persona("marco", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\marco.png");
            Aggiungi(tmp);
            tmp = new Persona("maria", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\maria.png");
            Aggiungi(tmp);
            tmp = new Persona("paolo", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\paolo.png");
            Aggiungi(tmp);

            tmp = new Persona("pietro", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\pietro.png");
            Aggiungi(tmp);
            tmp = new Persona("riccardo", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\riccardo.png");
            Aggiungi(tmp);
            tmp = new Persona("roberto", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\roberto.png");
            Aggiungi(tmp);
            tmp = new Persona("samuele", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\samuele.png");
            Aggiungi(tmp);
            tmp = new Persona("susana", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\susanna.png");
            Aggiungi(tmp);
            tmp = new Persona("tommaso", Directory.GetCurrentDirectory() + "\\Immagini\\Personaggi\\tommaso.png");
            Aggiungi(tmp);





        }
    }
}
