using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

namespace DenizProbabilityTest
{
    class Program
    {
        const string TARGET = "deniz";
        const string ALPHABET = "abcçdefgğhıijklmnoöprsştuüvyz"; // 29 harf

        static void Main()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════════╗");
            Console.WriteLine("║   'DENİZ' Kelimesi Rastgele Oluşturma Testi      ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════╝\n");

            Console.WriteLine($"Hedef: '{TARGET}'");
            Console.WriteLine($"Alfabe: {ALPHABET.Length} harf");
            Console.WriteLine($"Teorik olasılık: 1/{Math.Pow(29, 5):N0} ≈ {(1.0 / Math.Pow(29, 5)) * 100:E2}%\n");

            // Farklı test senaryoları
            Console.WriteLine("Hangi testi çalıştırmak istersiniz?");
            Console.WriteLine("1. Hızlı test (1 milyon deneme)");
            Console.WriteLine("2. Orta test (10 milyon deneme)");
            Console.WriteLine("3. Uzun test (100 milyon deneme)");
            Console.WriteLine("4. Paralel çok uzun test (1 milyar deneme - önerilen!)");
            Console.WriteLine("5. Sürekli test (bulana kadar çalış)\n");

            Console.Write("Seçiminiz (1-5): ");
            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    RunTest(1_000_000, false);
                    break;
                case "2":
                    RunTest(10_000_000, false);
                    break;
                case "3":
                    RunTest(100_000_000, false);
                    break;
                case "4":
                    RunParallelTest(1_000_000_000);
                    break;
                case "5":
                    RunUntilFound();
                    break;
                default:
                    Console.WriteLine("Geçersiz seçim! Hızlı test başlatılıyor...");
                    RunTest(1_000_000, false);
                    break;
            }
        }

        static void RunTest(long attempts, bool showProgress)
        {
            Console.WriteLine($"🎲 {attempts:N0} deneme yapılıyor...\n");

            var sw = Stopwatch.StartNew();
            long foundCount = 0;
            long checkInterval = attempts / 100; // %1'lik dilimler

            var random = new Random();
            var buffer = new char[5];

            for (long i = 0; i < attempts; i++)
            {
                // Rastgele 5 harf oluştur
                for (int j = 0; j < 5; j++)
                {
                    buffer[j] = ALPHABET[random.Next(ALPHABET.Length)];
                }

                string word = new(buffer);

                if (word == TARGET)
                {
                    foundCount++;
                    Console.WriteLine($"✨ BULUNDU! Deneme #{i + 1:N0}: '{word}'");
                }

                // İlerleme göster
                if (showProgress && i > 0 && i % checkInterval == 0)
                {
                    double progress = (i / (double)attempts) * 100;
                    Console.Write($"\rİlerleme: {progress:F1}% ({i:N0}/{attempts:N0})");
                }
            }

            sw.Stop();

            Console.WriteLine(showProgress ? "\n" : "");
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine($"⏱️  Süre: {sw.Elapsed.TotalSeconds:F2} saniye");
            Console.WriteLine($"🎯 Toplam deneme: {attempts:N0}");
            Console.WriteLine($"✅ Bulunan: {foundCount}");
            Console.WriteLine($"📊 Gerçek oran: {(foundCount / (double)attempts) * 100:E2}%");
            Console.WriteLine($"⚡ Hız: {attempts / sw.Elapsed.TotalSeconds:N0} deneme/saniye");
            Console.WriteLine("═══════════════════════════════════════════════════\n");

            if (foundCount == 0)
            {
                Console.WriteLine("💡 İpucu: 'deniz' bulmak için ~20 milyon deneme gerekiyor!");
                Console.WriteLine("   Daha fazla deneme için paralel testi deneyin (Seçenek 4)\n");
            }
        }

        static void RunParallelTest(long totalAttempts)
        {
            Console.WriteLine($"🚀 {totalAttempts:N0} deneme PARALEL olarak yapılıyor...");
            Console.WriteLine($"🔧 CPU Çekirdek sayısı: {Environment.ProcessorCount}\n");

            var sw = Stopwatch.StartNew();
            long foundCount = 0;
            long completedAttempts = 0;

            var lockObj = new object();
            int threadCount = Environment.ProcessorCount;
            long attemptsPerThread = totalAttempts / threadCount;

            Parallel.For(0, threadCount, threadIndex =>
            {
                var random = new Random(Guid.NewGuid().GetHashCode());
                var buffer = new char[5];
                long localFound = 0;
                long localAttempts = attemptsPerThread;

                // Son thread kalan denemeleri alsın
                if (threadIndex == threadCount - 1)
                {
                    localAttempts = totalAttempts - (attemptsPerThread * (threadCount - 1));
                }

                for (long i = 0; i < localAttempts; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        buffer[j] = ALPHABET[random.Next(ALPHABET.Length)];
                    }

                    string word = new(buffer);

                    if (word == TARGET)
                    {
                        localFound++;
                        lock (lockObj)
                        {
                            long globalAttempt = completedAttempts + i;
                            Console.WriteLine($"\n✨ BULUNDU! Thread #{threadIndex}, Deneme #{globalAttempt:N0}: '{word}'");
                        }
                    }

                    // İlerleme (her thread 10 milyonda bir rapor)
                    if (i > 0 && i % 10_000_000 == 0)
                    {
                        lock (lockObj)
                        {
                            completedAttempts += 10_000_000;
                            double progress = (completedAttempts / (double)totalAttempts) * 100;
                            Console.Write($"\rİlerleme: {progress:F1}% ({completedAttempts:N0}/{totalAttempts:N0})");
                        }
                    }
                }

                lock (lockObj)
                {
                    foundCount += localFound;
                    completedAttempts += localAttempts % 10_000_000;
                }
            });

            sw.Stop();

            Console.WriteLine("\n");
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine($"⏱️  Süre: {sw.Elapsed.TotalSeconds:F2} saniye ({sw.Elapsed:g})");
            Console.WriteLine($"🎯 Toplam deneme: {totalAttempts:N0}");
            Console.WriteLine($"✅ Bulunan: {foundCount}");
            Console.WriteLine($"📊 Gerçek oran: {(foundCount / (double)totalAttempts) * 100:E6}%");
            Console.WriteLine($"📈 Teorik beklenen: {totalAttempts / Math.Pow(29, 5):F2}");
            Console.WriteLine($"⚡ Hız: {totalAttempts / sw.Elapsed.TotalSeconds:N0} deneme/saniye");
            Console.WriteLine("═══════════════════════════════════════════════════\n");
        }

        static void RunUntilFound()
        {
            Console.WriteLine("🔄 'deniz' bulunana kadar sürekli deneme yapılıyor...");
            Console.WriteLine("   (Ctrl+C ile durdurun)\n");

            var sw = Stopwatch.StartNew();
            long attempts = 0;
            var random = new Random();
            var buffer = new char[5];

            while (true)
            {
                attempts++;

                for (int j = 0; j < 5; j++)
                {
                    buffer[j] = ALPHABET[random.Next(ALPHABET.Length)];
                }

                string word = new(buffer);

                if (word == TARGET)
                {
                    sw.Stop();
                    Console.WriteLine($"\n🎉🎉🎉 BULUNDU! 🎉🎉🎉");
                    Console.WriteLine($"✨ Kelime: '{word}'");
                    Console.WriteLine($"🎯 Deneme sayısı: {attempts:N0}");
                    Console.WriteLine($"⏱️  Süre: {sw.Elapsed.TotalSeconds:F2} saniye");
                    Console.WriteLine($"⚡ Hız: {attempts / sw.Elapsed.TotalSeconds:N0} deneme/saniye\n");
                    break;
                }

                // Her 1 milyon denemede ilerleme göster
                if (attempts % 1_000_000 == 0)
                {
                    Console.Write($"\rDeneme: {attempts:N0} - Süre: {sw.Elapsed.TotalSeconds:F0}s - Hız: {attempts / sw.Elapsed.TotalSeconds:N0}/s");
                }
            }

            Console.WriteLine("Test tamamlandı!");
        }
    }
}