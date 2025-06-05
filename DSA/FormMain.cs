using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSA
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        private BigInteger publicKey, privateKey, n;
        private Random random = new Random();

        /*
        █▀▀ █▀▀ █▄░█ █▀▀ █▀█ ▄▀█ ▀█▀ █▀▀   █▄▀ █▀▀ █▄█ █▀   █▄▄ █░█ ▀█▀ ▀█▀ █▀█ █▄░█
        █▄█ ██▄ █░▀█ ██▄ █▀▄ █▀█ ░█░ ██▄   █░█ ██▄ ░█░ ▄█   █▄█ █▄█ ░█░ ░█░ █▄█ █░▀█
         */
        private void btnGenerateKeys_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateKeys();
                txtPublicKey.Text = $"n: {n}, e: {publicKey}";
                txtPrivateKey.Text = $"n: {n}, d: {privateKey}";
                string privateKeyPath = "private_key.priv";
                string publicKeyPath = "public_key.pub";
                File.WriteAllText(privateKeyPath, $"RSA {Convert.ToBase64String(privateKey.ToByteArray())} {Convert.ToBase64String(n.ToByteArray())}");
                File.WriteAllText(publicKeyPath, $"RSA {Convert.ToBase64String(publicKey.ToByteArray())} {Convert.ToBase64String(n.ToByteArray())}");
                MessageBox.Show($"Klíče byly uloženy do souborů:\r\n{privateKeyPath}\r\n{publicKeyPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při generování klíče: {ex.Message}");
            }
        }

        /*
        █░░ █▀█ ▄▀█ █▀▄   █▄▄ █░█ ▀█▀ ▀█▀ █▀█ █▄░█
        █▄▄ █▄█ █▀█ █▄▀   █▄█ █▄█ ░█░ ░█░ █▄█ █░▀█
        */
        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "ZIP soubory|*.zip",
                    Title = "Načíst ZIP archiv"
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                string zipFilePath = openFileDialog.FileName;
                string extractPath = Path.Combine(Path.GetTempPath(), "ExtractedFiles");

                // Rozbalení obsahu ZIP archivu
                if (Directory.Exists(extractPath))
                    Directory.Delete(extractPath, true);

                ZipFile.ExtractToDirectory(zipFilePath, extractPath);

                // Kontrola požadovaných souborů

                string txtPath = Path.Combine(extractPath, "message.txt");
                string signPath = Path.Combine(extractPath, "message.txt.sign");

                if (!File.Exists(txtPath) || !File.Exists(signPath))
                {
                    throw new FileNotFoundException("Archiv neobsahuje všechny požadované soubory!");
                }

                // Načtení obsahu a zobrazení informací
                txtOutput.Text = File.ReadAllText(txtPath);

                // Zobrazení detailů o souborech
                FileInfo txtInfo = new FileInfo(txtPath);
                FileInfo signInfo = new FileInfo(signPath);

                txtFileDetails.Text = $"Název ZIP archivu: {Path.GetFileName(zipFilePath)}\r\n" +
                                      $"Cesta: {zipFilePath}\r\n\r\n" +
                                      $"-- Soubory v archivu --\r\n" +
                                      $"    Název: {txtInfo.Name}\r\n" +
                                      $"    Cesta: {txtInfo.FullName}\r\n" +
                                      $"    Velikost: {txtInfo.Length} bajtů\r\n" +
                                      $"    Datum vytvoření: {txtInfo.CreationTime}\r\n\r\n" +
                                      $"Signature:\r\n" +
                                      $"    Název: {signInfo.Name}\r\n" +
                                      $"    Cesta: {signInfo.FullName}\r\n" +
                                      $"    Velikost: {signInfo.Length} bajtů\r\n" +
                                      $"    Datum vytvoření: {signInfo.CreationTime}\r\n\r\n" +

                MessageBox.Show("ZIP archiv byl úspěšně načten!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání ZIP: {ex.Message}");
            }
        }
        /*
        █▀▀ ▀▄▀ █▀█ █▀█ █▀█ ▀█▀   █▄▄ █░█ ▀█▀ ▀█▀ █▀█ █▄░█
        ██▄ █░█ █▀▀ █▄█ █▀▄ ░█░   █▄█ █▄█ ░█░ ░█░ █▄█ █░▀█
         */
        private void btnExportZip_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtOutput.Text))
                {
                    MessageBox.Show("Výstupní textové pole je prázdné!");
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "ZIP soubory|*.zip",
                    Title = "Export jako ZIP"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                string zipFilePath = saveFileDialog.FileName;
                string tempFolder = Path.Combine(Path.GetTempPath(), "ExportedFiles");

                // Vytvoření dočasné složky
                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);

                Directory.CreateDirectory(tempFolder);

                // Uložení souborů do dočasné složky
                string messagePath = Path.Combine(tempFolder, "message.txt");
                File.WriteAllText(messagePath, txtOutput.Text);

                // Podepsání a přidání podpisu do ZIP
                byte[] fileContent = File.ReadAllBytes(messagePath);
                byte[] hash = ComputeSHA3Hash(fileContent);
                BigInteger hashBigInt = new BigInteger(hash.Reverse().ToArray());
                BigInteger signature = BigInteger.ModPow(hashBigInt, privateKey, n);
                string signPath = Path.Combine(tempFolder, "message.txt.sign");
                File.WriteAllText(signPath, $"RSA_SHA3-512 {Convert.ToBase64String(signature.ToByteArray())}");

                // Vytvoření ZIP archivu
                ZipFile.CreateFromDirectory(tempFolder, zipFilePath);

                MessageBox.Show("ZIP archiv byl úspěšně vytvořen!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při exportu ZIP: {ex.Message}");
            }
        }

        /*
        █░░ █▀█ ▄▀█ █▀▄   █▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▄▄ █░█ ▀█▀ ▀█▀ █▀█ █▄░█
        █▄▄ █▄█ █▀█ █▄▀   █▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █▄█ █▄█ ░█░ ░█░ █▄█ █░▀█
         */
        private void btnLoadPrivateKey_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Soukromé klíče|*.priv",
                    Title = "Načíst soukromý klíč"
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                string[] privateKeyParts = File.ReadAllText(openFileDialog.FileName).Split(' ');
                if (privateKeyParts.Length != 3 || privateKeyParts[0] != "RSA")
                    throw new InvalidOperationException("Neplatný formát soukromého klíče!");

                privateKey = new BigInteger(Convert.FromBase64String(privateKeyParts[1]));
                n = new BigInteger(Convert.FromBase64String(privateKeyParts[2]));

                txtPrivateKey.Text = $"n: {n}, d: {privateKey}";
                MessageBox.Show("Soukromý klíč byl načten!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání soukromého klíče: {ex.Message}");
            }
        }

        /*
        █░░ █▀█ ▄▀█ █▀▄   █▀█ █░█ █▄▄ █░░ █ █▀▀   █▄▄ █░█ ▀█▀ ▀█▀ █▀█ █▄░█
        █▄▄ █▄█ █▀█ █▄▀   █▀▀ █▄█ █▄█ █▄▄ █ █▄▄   █▄█ █▄█ ░█░ ░█░ █▄█ █░▀█
         */
        private void btnLoadPublicKey_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Veřejné klíče|*.pub",
                    Title = "Načíst veřejný klíč"
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                string[] publicKeyParts = File.ReadAllText(openFileDialog.FileName).Split(' ');
                if (publicKeyParts.Length != 3 || publicKeyParts[0] != "RSA")
                    throw new InvalidOperationException("Neplatný formát veřejného klíče!");

                publicKey = new BigInteger(Convert.FromBase64String(publicKeyParts[1]));
                n = new BigInteger(Convert.FromBase64String(publicKeyParts[2]));

                txtPublicKey.Text = $"n: {n}, e: {publicKey}";
                MessageBox.Show("Veřejný klíč byl načten!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání veřejného klíče: {ex.Message}");
            }
        }

        /*
        █░█ █▀▀ █▀█ █ █▀▀ █▄█   █▀ █ █▀▀ █▄░█ ▄▀█ ▀█▀ █░█ █▀█ █▀▀   █▄▄ █░█ ▀█▀ ▀█▀ █▀█ █▄░█
        ▀▄▀ ██▄ █▀▄ █ █▀░ ░█░   ▄█ █ █▄█ █░▀█ █▀█ ░█░ █▄█ █▀▄ ██▄   █▄█ █▄█ ░█░ ░█░ █▄█ █░▀█
         */

        private void btnVerifySignature_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtOutput.Text) || string.IsNullOrEmpty(txtPublicKey.Text))
                {
                    MessageBox.Show("Nejprve načtěte ZIP archiv obsahující všechny potřebné soubory!");
                    return;
                }

                string messagePath = Path.Combine(Path.GetTempPath(), "ExtractedFiles", "message.txt");
                string signaturePath = Path.Combine(Path.GetTempPath(), "ExtractedFiles", "message.txt.sign");

                if (!File.Exists(messagePath) || !File.Exists(signaturePath))
                {
                    MessageBox.Show("Chybí potřebné soubory (message.txt nebo message.txt.sign)!");
                    return;
                }

                // Načtení obsahu původního souboru a podpisu
                byte[] fileContent = File.ReadAllBytes(messagePath);
                string signatureContent = File.ReadAllText(signaturePath);
                string base64Signature = signatureContent.Split(' ')[1];
                BigInteger signature = new BigInteger(Convert.FromBase64String(base64Signature));

                // Vypočítání hash původního souboru
                byte[] hash = ComputeSHA3Hash(fileContent);

                // Ověření podpisu
                BigInteger expectedHash = BigInteger.ModPow(signature, publicKey, n);

                if (new BigInteger(hash.Reverse().ToArray()) == expectedHash)
                {
                    MessageBox.Show("Podpis je platný!");
                }
                else
                {
                    MessageBox.Show("Podpis není platný!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při ověřování podpisu: {ex.Message}");
            }
        }

        /*
        █▀ █░█ ▄▀█ ▄▄ █▀ ▄█ ▀█
        ▄█ █▀█ █▀█ ░░ ▄█ ░█ █▄
         */
        private byte[] ComputeSHA3Hash(byte[] data)
        {
            using (var sha3 = SHA512.Create())
            {
                return sha3.ComputeHash(data);
            }
        }

        /*
        █▀▀ █▀▀ █▄░█ █▀▀ █▀█ ▄▀█ ▀█▀ █▀▀   █▄▀ █▀▀ █▄█ █▀
        █▄█ ██▄ █░▀█ ██▄ █▀▄ █▀█ ░█░ ██▄   █░█ ██▄ ░█░ ▄█
         */
        private void GenerateKeys()
        {
            var p = GeneratePrime();
            var q = GeneratePrime();

            n = p * q;
            var phi = (p - 1) * (q - 1);

            publicKey = 65537;
            if (GCD(publicKey, phi) != 1)
                throw new Exception("Nelze vygenerovat veřejný klíč!");

            privateKey = ModInverse(publicKey, phi);
        }

        /*
        █▀▀ █▀▀ █▄░█ █▀▀ █▀█ ▄▀█ ▀█▀ █▀▀   █▀█ █▀█ █ █▀▄▀█ █▀▀
        █▄█ ██▄ █░▀█ ██▄ █▀▄ █▀█ ░█░ ██▄   █▀▀ █▀▄ █ █░▀░█ ██▄
         */
        private BigInteger GeneratePrime()
        {
            while (true)
            {
                var candidate = BigInteger.Abs(new BigInteger(GetRandomBytes(128))) | 1;
                if (IsPrime(candidate))
                    return candidate;
            }
        }

        /*
        █▀▀ █░█ █▀▀ █▀▀ █▄▀   █▀█ █▀█ █ █▀▄▀█ █▀▀
        █▄▄ █▀█ ██▄ █▄▄ █░█   █▀▀ █▀▄ █ █░▀░█ ██▄
         */
        private bool IsPrime(BigInteger num, int k = 10)
        {
            if (num <= 1 || num % 2 == 0) return false;
            BigInteger d = num - 1;
            int s = 0;
            while (d % 2 == 0) { d /= 2; s++; }

            for (int i = 0; i < k; i++)
            {
                var a = BigInteger.ModPow(2 + random.Next() % (num - 3), d, num);
                if (a == 1 || a == num - 1) continue;
                bool composite = true;
                for (int r = 0; r < s - 1 && composite; r++)
                {
                    a = BigInteger.ModPow(a, 2, num);
                    if (a == num - 1) composite = false;
                }
                if (composite) return false;
            }
            return true;
        }

        /*
        █▀▀ █▀▀ █▀▄
        █▄█ █▄▄ █▄▀
         */
        private BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                var t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        /*
        █ █▄░█ █░█ █▀▀ █▀█ ▀█ █▄░█ █   █▀▄▀█ █▀█ █▀▄ █░█ █░░ █▀█
        █ █░▀█ ▀▄▀ ██▄ █▀▄ █▄ █░▀█ █   █░▀░█ █▄█ █▄▀ █▄█ █▄▄ █▄█
         */
        private BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            var (m0, x0, x1) = (m, BigInteger.Zero, BigInteger.One);
            while (a > 1)
            {
                var q = a / m;
                (m, a) = (a % m, m);
                (x0, x1) = (x1 - q * x0, x0);
            }
            return x1 < 0 ? x1 + m0 : x1;
        }

        /*
        █▄░█ ▄▀█ █░█ █▀█ █▀▄ █▄░█ █▄█   █▄▄ █▄█ ▀█▀ █▀▀
        █░▀█ █▀█ █▀█ █▄█ █▄▀ █░▀█ ░█░   █▄█ ░█░ ░█░ ██▄
         */
        private byte[] GetRandomBytes(int length)
        {
            var bytes = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }

        private void btnSignAndExport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Textové soubory|*.txt",
                    Title = "Vyberte soubor k podepsání"
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                string selectedFilePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(selectedFilePath);

                // Hash souboru
                byte[] fileBytes = File.ReadAllBytes(selectedFilePath);
                byte[] hash = ComputeSHA3Hash(fileBytes);
                BigInteger hashBigInt = new BigInteger(hash.Reverse().ToArray());

                // Podepsání souboru
                BigInteger signature = BigInteger.ModPow(hashBigInt, privateKey, n);
                string signatureString = $"RSA_SHA3-512 {Convert.ToBase64String(signature.ToByteArray())}";

                // Příprava exportu
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "ZIP soubory|*.zip",
                    Title = "Exportovat jako ZIP"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                string zipFilePath = saveFileDialog.FileName;
                string tempFolder = Path.Combine(Path.GetTempPath(), "SignAndExport");

                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);

                Directory.CreateDirectory(tempFolder);

                // Uložení původního souboru a podpisu
                string messagePath = Path.Combine(tempFolder, Path.GetFileName(selectedFilePath));
                string signPath = Path.Combine(tempFolder, Path.GetFileName(selectedFilePath) + ".sign");

                File.WriteAllText(messagePath, fileContent);
                File.WriteAllText(signPath, signatureString);

                // Vytvoření ZIP archivu
                ZipFile.CreateFromDirectory(tempFolder, zipFilePath);

                MessageBox.Show("Soubor byl úspěšně podepsán a exportován do ZIP archivu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba: {ex.Message}");
            }
        }

    }
}
