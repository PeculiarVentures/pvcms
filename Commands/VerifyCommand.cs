using System;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using CommandLine;

namespace pvcms.Commands
{
    [Verb("verify")]
    public class VerifyCommand : BaseCommand
    {
        [Value(0, Required = true)]
        public string File { get; set; }

        protected override void OnExecute()
        {
            var bytes = System.IO.File.ReadAllBytes(File);
            var raw = PemConverter.IsPEM(bytes)
                ? PemConverter.Decode(bytes)
                : bytes;

            var signedCms = new SignedCms();
            signedCms.Decode(raw);

            PrintInfoCms(signedCms);

            try
            {
                signedCms.CheckSignature(false);
                Print.Info("Valid signature.");
            } catch (Exception e)
            {
                Print.Info($"{e.Message}");
                Environment.Exit(1);
            }

        }

        public static void PrintInfoCms(SignedCms cms, int tabs = 0)
        {
            Print.WriteLine("Signed data:".Tabs(tabs));

            var content = cms.Detached ? "Detached" : "Attached";
            Print.WriteLine($"Content: {content}".Tabs(tabs + TabSize));

            foreach (var signerInfo in cms.SignerInfos)
            {
                Print.WriteLine();
                PrintSignerInfo(signerInfo);
            }
        }

        public static void PrintSignerInfo(SignerInfo signerInfo, int tabs = 0)
        {
            Print.WriteLine("Signer info:".Tabs(tabs));

            try
            {
                signerInfo.CheckSignature(false);
                Print.WriteLine("Signature: OK".Tabs(tabs + TabSize));
            }
            catch (Exception e)
            {
                Print.WriteLine($"Signature: {e.Message}".Tabs(tabs + TabSize));
            }

            PrintCertificate(signerInfo.Certificate, tabs + TabSize);

            foreach (var counterSignerInfo in signerInfo.CounterSignerInfos)
            {
                Print.WriteLine();
                PrintSignerInfo(counterSignerInfo, tabs + TabSize);
            }
        }

        public static void PrintCertificate(X509Certificate2 cert, int tabs = 0)
        {
            Print.WriteLine($"Serial number: {cert.SerialNumber}".Tabs(tabs));
            Print.WriteLine($"Subject: {cert.Subject}".Tabs(tabs));
            Print.WriteLine($"Issuer: {cert.Issuer}".Tabs(tabs));
            Print.WriteLine($"Not before: {cert.NotBefore}".Tabs(tabs));
            Print.WriteLine($"Not after: {cert.NotAfter}".Tabs(tabs));
            Print.WriteLine($"Thumbprint: {cert.Thumbprint}".Tabs(tabs));
        }
    }
}
