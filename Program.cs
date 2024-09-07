using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AsciiArt;

namespace jeu_du_pendu
{
    class Program
    {
        static void AfficherMot(string mot, List<char> lettres)
        {
            for (int i = 0; i< mot.Length; i++)
            {
                char lettre = mot[i]; // necessaire pour le passer dans le contains.... J'ai galéré juste sur ça en fait. -_-

                    if (lettres.Contains(lettre))
                    {
                        Console.Write(lettre + " ");
                    }
                    else
                    {
                        Console.Write("_ ");
                    }
            }
            Console.WriteLine();
        }

        static char DemanderLettre(string message = "Entrez une lettre : ")
        {
            while (true)
            {
                Console.Write(message);
                string reponse = Console.ReadLine();

                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                Console.WriteLine("ERREUR : Vous devez entrer une lettre.");
            }
        }

        static void DevinerMot(string mot)
        {
            var lettresDevinees = new List<char>();
            var lettresNonDevinees = new List<char>();
            const int NB_VIES = 6;
            int viesRestantes = NB_VIES;

            while (viesRestantes > 0)
            {
                Console.WriteLine(Ascii.PENDU[NB_VIES-viesRestantes]);
                Console.WriteLine();

                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                var lettre = DemanderLettre();
                Console.Clear();

                if (mot.Contains(lettre))
                {
                    Console.WriteLine(lettre + " est dans le mot.");
                    lettresDevinees.Add(lettre);

                    if (ToutesLettresDevinees(mot, lettresDevinees))
                    {
                        AfficherMot(mot,lettresDevinees);
                        Console.WriteLine();

                        Console.WriteLine("BRAVO ! Vous avez gagné !");
                        return;
                    }
                }
                else
                {
                    if (!lettresNonDevinees.Contains(lettre)) // Si la mauvaise lettre n'a pas déjà été entrée, alors ....
                    {
                        lettresNonDevinees.Add(lettre);
                        viesRestantes--;
                        Console.WriteLine("Il vous reste " + viesRestantes + " vies.");
                    }
                }

                if(lettresNonDevinees.Count > 0)
                {
                    Console.WriteLine("Les lettres suivantes ne sont pas dans le mot : " + string.Join(", ", lettresNonDevinees)); // Pour concaténer une liste dans ma string
                }
                Console.WriteLine();
            }

            Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);


            if (viesRestantes == 0)
            {
                Console.WriteLine("PERDU ! Le mot était : " + mot);
                return;
            }
        }

        static bool ToutesLettresDevinees(string mot, List<char> lettres)
        {
            for (int i = 0; i < lettres.Count; i++) // j'ai fait l'erreur de boucler sur la longueur du mot au lieu de liste de lettres.
            {
                mot = mot.Replace(lettres[i].ToString(), "");
            }
            if (mot == "")
            {
                return true;
            }
            return false; // J'avais oublié ce return le cas échéant.

        }

        static string[] ChargerLesMots(string nomDuFichier)
        {
            try
            {
            return File.ReadAllLines(nomDuFichier);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Erreur de lecture du fichier " + nomDuFichier + "(" + ex + ")");
            }
            return null;// mettre un return ! On gère cette exception dans le programme.
        }

        static bool DemanderDeRejouer()
        {
            Console.WriteLine();
            char reponse = DemanderLettre("Voulez-vous rejouer ? (o/n) : ");
            
            reponse.ToString().ToUpper(); // Test

            if ((reponse == 'O'))
            {
                return true;
            }
            else if ((reponse == 'N'))
            {
                return false;
            }
            else
            {
                Console.WriteLine("Vous devez répondre avec o ou n.");
                return DemanderDeRejouer();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("BIEVENUE DANS LE JEU DU PENDU.\nTROUVEZ LE MOT ET SAUVEZ LE PERSONNAGE DE LA PENDAISON !");
            while (true)
            {
                var mots = ChargerLesMots("mots.txt");
                Random r = new Random();
                int i = r.Next(mots.Length); // Rappel : Tableau = Length ; Liste = Count !

                if ((mots == null) || (mots.Length == 0))
                {
                    Console.WriteLine("La liste de mots est vide.");
                }
                else
                {
                    string mot = mots[i].Trim().ToUpper();
                    DevinerMot(mot);
                    if (!DemanderDeRejouer())
                    {
                        break;
                    }
                    Console.Clear();
                }
            }
            Console.WriteLine("\nMerci et à bientôt !");
        }
    }
}