using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class GameForm : Form
    {
        private bool movetotheRigth;
        private bool movetotheLeft;
        private bool isgameOver;
        private int score, ballX, ballY, speedPlayer;
        private int lives=3;
        private Random rnd = new Random(); 
        private PictureBox[] creatingBlocks;
        
        
        //Inicializa forma de Game y los Bloques que se ocuparán
        public GameForm()
        {
            InitializeComponent();
            placeBlocks();
        }

        //Funcionamiento del juego, muestra los scores y la cantidad de vidas disponibles
        private void mainGameTimerEvent(object sender, EventArgs e)
        {
             
            //string scoreText = score.ToString();
            
            if (movetotheLeft == true && Player.Left > 0)
            {
                Player.Left -= speedPlayer;
                
            } 
            if (movetotheRigth == true && Player.Left <654)
            {
                Player.Left += speedPlayer;
            }

            ball.Left += ballX;
            ball.Top += ballY;

            if (ball.Left < 0 || ball.Left > 777)
            {
                ballX = -ballX;
            }

            if (ball.Top < 0)
            {
                ballY = -ballY;
            }

            if (ball.Bounds.IntersectsWith(Player.Bounds))
            {
                ballY = rnd.Next(5, 12) * -1;

                if (ballX < 0)
                {
                    ballX = rnd.Next(5, 12) * -1;
                }
                else
                {
                    ballX = rnd.Next(5, 12);
                }
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string) x.Tag == "blocks")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 100;
                        ballY = -ballY;
                        this.Controls.Remove(x);
                    }
                }
            }
            
            if (score == 1500 ){
            //Gano el juego
            
            gameOver("You win!! See You soon!");
            }

            if (ball.Top > 489)
            {
             //Pierde una vida 
             lives -= 1;
             gameOver("You lose a life. You have "+ lives+ " lives left");

             if (lives == 0)
             {
                 //Pierde el juego
                 gameOver("GAME OVER");
             }
            }
            
        }

        //Finaliza el juego, crea un menssage para mostrar cuando el juego termina por vidas o por puntos
        private void gameOver(string message)
        {
            isgameOver = true;
            gameTime.Stop();

            //score.ToString("Score: " + score + " " + message);
        }

        //Crea los bloques para con ello poder reinicializar el juego
        private void placeBlocks()
        {
            creatingBlocks = new PictureBox[15];
            int a = 0;
            int top = 50;
            int left = 100;

            for (int i = 0; i < creatingBlocks.Length; i++)
            {
                creatingBlocks[i] = new PictureBox();
                creatingBlocks[i].Height = 32;
                creatingBlocks[i].Width = 100;
                creatingBlocks[i].Tag = "blocks";
                creatingBlocks[i].BackColor= Color.Fuchsia;

                if (a == 5)
                {
                    top = top + 50;
                    left = 100;
                    a = 0;
                }

                if (a < 5)
                {
                    a++;
                    creatingBlocks[i].Left = left;
                    creatingBlocks[i].Top = top;
                    this.Controls.Add(creatingBlocks[i]);
                    left = left + 130;
                    
                }

            }
            setUpGame();
        }

        /*En caso de que el usuario pierda por vidas, está función elimina los bloques sobrantes y permite
        que se inicie de nuevo el juego */
        private void removeBlocs()
        {
            foreach (PictureBox x in creatingBlocks)
            {
                this.Controls.Remove(x);
            }
        }

        //Inicia a correr el timer del Juego, lo que permite que se vea la animación
        private void setUpGame()
        {
            isgameOver = false;
            score = 0;
            ballX = 5;
            ballY = 5;
            speedPlayer = 12;

            ball.Left = 386;
            ball.Top = 385;
            Player.Left = 326;
            
            //score.ToString("Score: " + score);
           // string scoreText = score.ToString();
            gameTime.Start();

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string) x.Tag == "blocks")
                {
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256),rnd.Next(256));
                }
            }
        }
        
        
        //Crea el movimiento de la pieza Player hacía la izquierda
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                movetotheLeft = true;
                
            }

            if (e.KeyCode == Keys.Right)
            {
                movetotheRigth = true;
            }
        }
        
        /*Crea el movimiento de la pieza Player hacía la derecha, además de que crea el evento enter
        donde el usuario presina la tecla enter para reinicializar todo el juego */
        private void KeyisUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                movetotheLeft = false;
                
            }

            if (e.KeyCode == Keys.Right)
            {
                movetotheRigth = false;
                
            }

            if (e.KeyCode == Keys.Enter && isgameOver == true)
            {
                removeBlocs();
                placeBlocks();
            }
        }
    }
}