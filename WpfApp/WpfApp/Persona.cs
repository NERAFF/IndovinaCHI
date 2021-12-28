using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Persona
    {
        private String nome;
        private String percorso;
        private Boolean attivo;
        public Persona()
        {

        }
        public Persona(String nome, String percorso)
        {
            this.nome = nome;
            this.percorso = percorso;
            this.attivo = false;
        }

        public void setAttivo(Boolean attivo)
        {
            this.attivo = attivo;
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
    }
}
