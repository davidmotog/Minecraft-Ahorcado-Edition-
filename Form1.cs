using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using LoremNET;
using System.Text.RegularExpressions;
using System.Linq; //para avilitar el ordenby para poder cojer 1 palabra de la lista de mi array
using System.Text; //  StringBuilder adivinar = new StringBuilder(name);

namespace Ahorcado
{
    public partial class Form1 : Form
    {



        //string palabra, aleatorio;
        Random random = new Random(); // CREO UNA VARIABLE RANDom 

        //string personas2 = personas.OrderBy(cantidad => random.Next()).ToArray();
        //string name;
        string randomWord = "";
        //string randomWord2 = "", guessWord1;
        int numberGuesses = 0, wordLength, maxGuesses = 6, totalTries = 0;
        bool started = false;
        string[] randomWord4 = { "ajolote", "murcielago", "abeja", "blaze", "gato", "araña de cueva", "pollo", "bacalao", "vaca", "creeper", "delfin", "burro", "ahogado", "guardian anciano", "enderman", "endermite", "invocador", "zorro", "ghast", "calamar luminoso", "cabra", "guardian", "hoglin", "caballo", "zombie momificado", "llama", "cubo de magma", "champillaca", "mula", "ocelote", "panda", "loro", "fantasma", "cerdo", "piglin", "piglin bruto", "saqueador", "oso polar", "pez globo", "conejo", "devastador", "salmon", "oveja", "shulker", "lepisma", "esqueleto", "caballo esqueleto", "slime", "araña", "calamar", "esquelejo glacial", "lavagante", "llama ambulante", "pez tropical", "tortuga", "anima", "aldeano", "vindicador", "vendedor ambulante", "bruja", "esqueleto wither", "lovo", "zoglin", "zombi", "caballo zombi", "aldeano zombi", "piglin zombificado" /*, "gato", "araña de cueva", "pollo", "bacalao", "vaca", "creeper" */ };

        /// <summary>
        /// prueba gorda
        /// </summary>

        //string name = randomWord4[0];
        //StringBuilder adivinar = new StringBuilder(name);

        //Random random2 = new Random(); // CREO UNA VARIABLE RANDom 

        // Creo un aray
        //string[] randomWord = { "juan", "ana", "pedro", "eva", "paco", "Juanjo", "anabelle", "rosa", "elitros", "paca", "josias", "alicia", "padro", "jeidy", "david", "isabel", "alejandro", "francisco", "joseph", "stefano" };
        //string[] personas = { "juan", "ana", "pedro", "eva", "paco", "Juanjo", "anabelle", "rosa", "elitros", "paca", "josias", "alicia", "padro", "jeidy", "david", "isabel", "alejandro", "francisco"
        //personas = personas.OrderBy(cantidad => random.Next()).ToArray();
        //string[] randomWord = randomWord.OrderBy(cantidad => random.Next()).ToArray();
        /// codigo-original
        //randomWord = LoremNET.Lorem.Words(1).ToString().ToLower();
        //randomWord = personas.OrderBy(cantidad => random.Next()).ToArray();
        // debug.Text = Convert.ToString(randomWord);
        // debug.Text = randomWord
        //string randomWord2 = randomWord[0];
        // StringBuilder adivinar = new StringBuilder(randomWord2);
        // debug.Text = randomWord2;

        //Console.WriteLine(debug.Text);

        public void GameStart()
        {
            numberGuesses = 0;
            totalTries = 0;

            Random random = new Random(); // CREO UNA VARIABLE RANDom 

            // Creo un aray
            //string[] randomWord = { "juan", "ana", "pedro", "eva", "paco", "Juanjo", "anabelle", "rosa", "elitros", "paca", "josias", "alicia", "padro", "jeidy", "david", "isabel", "alejandro", "francisco", "joseph", "stefano" };
            //string[] personas = { "juan", "ana", "pedro", "eva", "paco", "Juanjo", "anabelle", "rosa", "elitros", "paca", "josias", "alicia", "padro", "jeidy", "david", "isabel", "alejandro", "francisco"
            //personas = personas.OrderBy(cantidad => random.Next()).ToArray();
            // randomWord = randomWord.OrderBy(cantidad => random.Next()).ToArray();
            /// codigo-original
            //randomWord = LoremNET.Lorem.Words(1).ToString().ToLower();
            //randomWord = personas.OrderBy(cantidad => random.Next()).ToArray();
            // debug.Text = Convert.ToString(randomWord);
            // debug.Text = randomWord
            // string randomWord2 = randomWord[0];
            // StringBuilder adivinar = new StringBuilder(randomWord2);
            // StringBuilder adivinar2 = new StringBuilder(name);

            randomWord4 = randomWord4.OrderBy(cantidad => random.Next()).ToArray(); // funcionara ?

            string name = randomWord4[0];
            StringBuilder adivinar = new StringBuilder(name);



            debug.Text = name;

            //Console.WriteLine(debug.Text);
            InitialBox();
            started = true;
            doguess.Enabled = true;
            mostrar.Enabled = true;
            input.Enabled = true;
            Object rm = Properties.Resources.ResourceManager.GetObject("Hangman-0"); // no se que hace
            Bitmap myImage = (Bitmap)rm; // no se que hace
            hangman.Image = myImage;
        }

        public void Guessing(string guessedword) //Doing the guesswork
        {
            totalTries++;
            timesGuessed.Text = $"Total Intentos: {totalTries}";
            if (guessedword.Length == 1)
            {
                // Do one letter guess
                string compare1 = guess.Text;
                char[] toChange = guess.Text.ToCharArray();

                string name = randomWord4[0];
                StringBuilder adivinar = new StringBuilder(name);

                Regex rgx = new Regex(guessedword);
                //string randomWord3 = Convert.ToString(randomWord2);
                foreach (Match match in rgx.Matches(name))
                {
                    debug.Text = match.Index.ToString();

                    toChange[match.Index * 2 + 1] = Convert.ToChar(match.Value);
                }
                string changed = new string(toChange);
                debug.Text = changed;
                guess.Text = changed;
                if (changed == compare1)
                {
                    WrongGuess();
                }
                if (changed.Replace(" ", "") == name)
                {
                    Won();
                }

            }
            else if (guessedword.Length == wordLength)
            {
                if (guessedword == randomWord)
                {
                    Won();
                }
                else
                {
                    WrongGuess();
                }
            }
            else
            {
                //Incorrect
                WrongGuess();
            }
            if (numberGuesses == maxGuesses)
            {
                Lost();
            }
        }

        public void WrongGuess()
        {
            //numberGuesses++;
            // tries.Text = $"Intentos fallidos: {numberGuesses}";
            numberGuesses++;
            Object rm = Properties.Resources.ResourceManager.GetObject($"Hangman-{numberGuesses}");
            Bitmap myImage = (Bitmap)rm;
            hangman.Image = myImage;
        }

        public void Won()
        {
            string name = randomWord4[0];
            StringBuilder adivinar = new StringBuilder(name);
            MessageBox.Show($"¡Has ganadado! La palabra era: {name}.");
            guess.Text = randomWord;
            GameEnded();

        }

        public void Lost()
        {

            string name = randomWord4[0];
            StringBuilder adivinar = new StringBuilder(name);

            //  string randomWord3 = Convert.ToString(randomWord2);
            //string randomWord5 = Convert.ToString(randomWord4);
            //StringBuilder adivinar = new StringBuilder(randomWord4);
            //randomWord4 = randomWord4.OrderBy(cantidad => random.Next()).ToArray();
            //string name = randomWord4[0];
            /// StringBuilder adivinar = new StringBuilder(name);

            //MessageBox.Show($"Has perdido! La palabra era: {randomWord2}. :(");
            MessageBox.Show($"Has perdido! La palabra era: {name}. :(");
            // MessageBox.Show($"Has perdido! La palabra era: {randomWord3}. :(");
            guess.Text = "Pulsa Comenzar para probar otra vez.";
            GameEnded();
        }

        public void GameEnded()
        {
            start.Enabled = true;
            mostrar.Enabled = false;
            doguess.Enabled = false;
            input.Enabled = false;
            started = false;
        }

        private void mostrar_Click(object sender, EventArgs e)
        {
            Lost();
        }

        public Form1()
        {
            InitializeComponent();
            Text += " · Alfa 2.0";
        }

        private void guess_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Bitmap img = new Bitmap(Application.StartupPath + @"\img\fondo.jpg");
            //BackgroundImage = img;
        }

        private void hangman_Click(object sender, EventArgs e)
        {

        }

        private void input_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("El juego usa Solamente los Mobs del juego , para jugar , introducir letra a letra hasta el final ", "Ayuda");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show($"Verion del juego alfa 1.0 estable");
            MessageBox.Show("Versión del juego Alfa 2.0", "Acerca De");

        }

        private void doguess_Click(object sender, EventArgs e)
        {
            Guessing(input.Text);
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Close();
        }



        public void InitialBox()
        {
            guess.Text = "";
            //debug.Text = "";
            ///
            // a lo bruto
            ///

            // Random random = new Random(); // CREO UNA VARIABLE RANDom 






            // Creo un aray
            // string[] randomWord = { "juan", "ana", "pedro", "eva", "paco", "Juanjo", "anabelle", "rosa", "elitros", "paca", "josias", "alicia", "padro", "jeidy", "david", "isabel", "alejandro", "francisco", "joseph", "stefano" };
            //string[] personas = { "juan", "ana", "pedro", "eva", "paco", "Juanjo", "anabelle", "rosa", "elitros", "paca", "josias", "alicia", "padro", "jeidy", "david", "isabel", "alejandro", "francisco"
            //personas = personas.OrderBy(cantidad => random.Next()).ToArray();
            // randomWord = randomWord.OrderBy(cantidad => random.Next()).ToArray();
            /// codigo-original
            //randomWord = LoremNET.Lorem.Words(1).ToString().ToLower();
            //randomWord = personas.OrderBy(cantidad => random.Next()).ToArray();
            // debug.Text = Convert.ToString(randomWord);
            // debug.Text = randomWord
            //string randomWord2 = randomWord[0];
            //string randomWord3 = Convert.ToString(randomWord2);
            // randomWord3 = randomWord[0];
            // StringBuilder adivinar = new StringBuilder(randomWord3);
            // StringBuilder adivinar2 = new StringBuilder(name);
            //debug.Text = randomWord2;

            //int longitud = name.Length;
            //int longitud = randomWord4.Length;


            //guess.Text = randomWord2;


            string name = randomWord4[0];
            StringBuilder adivinar = new StringBuilder(name);




            wordLength = name.Length;



            for (int i = 0; i < wordLength; i++)
            {

                guess.Text = guess.Text + " _";
            }

            // for (int i = 0; i < wordLength; i++)
            // {
            //guess.Text = guess.Text + " _";

            //  if (i == longitud || i == wordLength - 1)
            //  {
            //adivinar[i] = name[i];
            //adivinar[i] = name[i];
            //adivinar[i] = '_';
            //guess.Text = guess.Text + " _";
            // guess.Text = Convert.ToString(adivinar[i]);
            //   }
            //   else
            //   {
            //adivinar[i] = '_'; // crea el skeleto inicial del programa
            //      guess.Text = guess.Text + " _";
            //  }

            // }



            // for (int i = 0; i < randomWord2.Length; i++) // comprueba la longitud de la palabra seleccionada
            // {
            //char[] adivinar = Convert.ToChar(personas); 
            //  if (i == longitud || i == randomWord2.Length - 1)
            //  {
            //adivinar[i] = name[i];
            //   adivinar[i] = randomWord2[i];
            //guess.Text = adivinar[i];
            //adivinar[i] = '_';
            //  }
            //  else
            // {
            //     adivinar[i] = '_'; // crea el skeleto inicial del programa
            // }


            //  for (int a = 0; a < randomWord2.Length; a++)
            // {
            //Console.Write(" {0}", adivinar[a]);
            //guess.Text = adivinar[a];
            //   guess.Text = Convert.ToString(adivinar[a]);
            //  }

            // }





        }

        private void start_Click(object sender, EventArgs e)

        {

            guess.Text = "";
            //input_Text.Clear();
            if (started)
            {
                if (MessageBox.Show("¿Seguro que quieres reiniciar el juego?", "Reiniciar el juego?", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    GameStart();
                }
            }
            else
            {
                GameStart();
            }

        }

    }
}
