using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class Persona
    {
        public String nome;
        public String percorso;
        public Boolean attivo;
        public Boolean eliminato;
        public Persona()
        {
            nome = "";
            percorso = "";
            attivo = true;
            eliminato = true;
        }
        public Persona(String nome, String percorso)
        {
            this.nome = nome;
            this.percorso = percorso;
            this.attivo = true;
            this.eliminato = true;
        }

        public void setAttivo(Boolean attivo)
        {
            this.attivo = attivo;
        }
        public void setEliminato(Boolean eliminato)
        {
            this.eliminato = eliminato;
        }
        public void setPercorso(String percorso)
        {
            this.percorso = percorso;
        }

        public String getNome()
        {
            return nome;
        }

        public String getPercorso()
        {
            return percorso;
        }

        public Boolean isAttivo()
        {
            return attivo;
        }
        public Boolean isEliminato()
        {
            return eliminato;
        }
    }
}
