# 🎲 Deniz Olasılık Testi

Rastgele 5 harf oluşturarak "deniz" kelimesini bulma olasılığını test eden yüksek performanslı C# konsol uygulaması.

## 📋 İçindekiler

- [Proje Hakkında](#proje-hakkında)
- [Matematiksel Temeli](#matematiksel-temeli)
- [Özellikler](#özellikler)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [Test Modları](#test-modları)
- [Sonuçlar](#sonuçlar)
- [Teknik Detaylar](#teknik-detaylar)
- [Performans](#performans)

## 🎯 Proje Hakkında

Bu proje, olasılık teorisini pratikte test etmek için geliştirilmiştir. Türkçe alfabesinden (29 harf) rastgele 5 harf seçerek "deniz" kelimesinin oluşma olasılığını hesaplar ve gerçek deneylerle doğrular.

### Soru
> "5 harfi sırayla rastgele oluşturacak bir algoritmada 'deniz' yazma ihtimali nedir?"

### Cevap
**1 / 20,511,149** (yaklaşık 20 milyonda 1)

## 📐 Matematiksel Temeli

### Teorik Hesaplama

```
Her pozisyon için 29 olası harf var
P(deniz) = (1/29) × (1/29) × (1/29) × (1/29) × (1/29)
         = 1/29⁵
         = 1/20,511,149
         ≈ 0.00000487%
```

### Karşılaştırma

Bu olasılık:
- 🎰 Piyangoda büyük ikramiye kazanmaktan (~14 milyonda 1) **daha düşük**
- ⚡ Yıldırım çarpmalarından (~300,000'de 1) **çok daha düşük**
- 🎂 Aynı günde 4 kişinin doğum günü tutmasından (~133,000'de 1) **daha düşük**

## ✨ Özellikler

- ⚡ **Yüksek Performans**: Saniyede 140+ milyon deneme
- 🔀 **Paralel İşleme**: Tüm CPU çekirdeklerini kullanır
- 📊 **Gerçek Zamanlı İstatistikler**: Anlık ilerleme ve metrikler
- 🎯 **5 Farklı Test Modu**: Hızlıdan sürekli teste kadar
- 🇹🇷 **Türkçe Karakter Desteği**: 29 harfli Türkçe alfabe
- 📈 **Detaylı Raporlama**: Teorik vs gerçek sonuç karşılaştırması

## 🚀 Kurulum

### Gereksinimler

- .NET 10.0 veya üzeri
- Windows / Linux / macOS

### Adımlar

```bash
# Projeyi klonlayın
git clone https://github.com/yourusername/deniz-probability-test.git

# Proje dizinine gidin
cd deniz-probability-test

# Uygulamayı çalıştırın
dotnet run
```

veya

```bash
# Derleyin
dotnet build -c Release

# Çalıştırın
dotnet run -c Release
```

## 💻 Kullanım

Program başlatıldığında 5 test modu sunulur:

```
Hangi testi çalıştırmak istersiniz?
1. Hızlı test (1 milyon deneme)
2. Orta test (10 milyon deneme)
3. Uzun test (100 milyon deneme)
4. Paralel çok uzun test (1 milyar deneme - önerilen!)
5. Sürekli test (bulana kadar çalış)

Seçiminiz (1-5): _
```

## 📊 Test Modları

### 1️⃣ Hızlı Test (1M deneme)
- **Süre**: ~1 saniye
- **Beklenen bulma**: 0 (çok düşük olasılık)
- **Kullanım**: Hızlı performans testi

### 2️⃣ Orta Test (10M deneme)
- **Süre**: ~10 saniye
- **Beklenen bulma**: 0-1
- **Kullanım**: Orta seviye test

### 3️⃣ Uzun Test (100M deneme)
- **Süre**: ~30 saniye
- **Beklenen bulma**: 4-5
- **Kullanım**: İstatistiksel doğrulama

### 4️⃣ Paralel Test (1B deneme) ⭐ ÖNERİLEN
- **Süre**: ~7 saniye (20 çekirdekli CPU'da)
- **Beklenen bulma**: 48-49
- **Kullanım**: En doğru istatistiksel sonuç
- **Özellik**: Tüm CPU çekirdeklerini kullanır

### 5️⃣ Sürekli Test
- **Süre**: "deniz" bulunana kadar
- **Ortalama**: 20 milyon deneme (~0.7 saniye)
- **Kullanım**: Garantili bulma

## 📈 Sonuçlar

### Örnek Çıktı (1 Milyar Deneme)

```
🚀 1.000.000.000 deneme PARALEL olarak yapılıyor...
🔧 CPU Çekirdek sayısı: 20

✨ BULUNDU! Thread #11, Deneme #1.706.619: 'deniz'
✨ BULUNDU! Thread #1, Deneme #3.378.567: 'deniz'
...
✨ BULUNDU! Thread #9, Deneme #828.846.016: 'deniz'

═══════════════════════════════════════════════════
⏱️  Süre: 7,10 saniye
🎯 Toplam deneme: 1.000.000.000
✅ Bulunan: 58
📊 Gerçek oran: 5,800000E-006%
📈 Teorik beklenen: 48,75
⚡ Hız: 140.821.690 deneme/saniye
═══════════════════════════════════════════════════
```

### Analiz

- **Teorik**: 48.75 bulma beklenir
- **Gerçek**: 58 bulma (19% şanslı!)
- **Ortalama**: Her 17.2 milyon denemede 1 bulma
- **Sapma**: İstatistiksel olarak normal

## 🔧 Teknik Detaylar

### Kullanılan Teknolojiler

- **Dil**: C# 12.0
- **Framework**: .NET 10.0
- **Paralel İşleme**: `Parallel.For` (Task Parallel Library)
- **Random**: `System.Random` (Thread-safe kullanım)

### Algoritma

```csharp
// Her thread için
for (long i = 0; i < attempts; i++)
{
    // 5 rastgele harf oluştur
    for (int j = 0; j < 5; j++)
    {
        buffer[j] = ALPHABET[random.Next(29)];
    }
    
    // Kontrolet
    if (new string(buffer) == "deniz")
        foundCount++;
}
```

### Thread Güvenliği

```csharp
// Her thread kendi Random instance'ını kullanır
var random = new Random(Guid.NewGuid().GetHashCode());

// Kritik bölgeler için lock
lock (lockObj)
{
    foundCount++;
    Console.WriteLine($"Bulundu: {word}");
}
```

## ⚡ Performans

### Benchmark Sonuçları

| CPU | Çekirdek | Test | Süre | Hız (deneme/s) |
|-----|----------|------|------|----------------|
| AMD Ryzen 9 | 20 | 1B | 7.1s | 140M |
| Intel i7 | 8 | 1B | 15s | 66M |
| Intel i5 | 4 | 1B | 30s | 33M |

### Optimizasyonlar

1. **Paralel İşleme**: CPU çekirdeklerinin tamamını kullanma
2. **Buffer Kullanımı**: String allocation yerine char array
3. **Thread-Local Random**: Her thread kendi RNG'sine sahip
4. **Lock Minimizasyonu**: Sadece kritik bölgelerde lock

### Bellek Kullanımı

- **Tek thread**: ~10 MB
- **20 thread**: ~50 MB
- **GC pressure**: Minimal (buffer reuse)

## 📚 Öğrenim Değeri

Bu proje şunları gösterir:

1. **Olasılık Teorisi**: Matematiksel hesaplamaların pratikte doğrulanması
2. **Büyük Sayılar Kanunu**: Deneme sayısı arttıkça teoriye yakınsama
3. **Paralel Programlama**: CPU çekirdeklerini verimli kullanma
4. **Performans Optimizasyonu**: Yüksek hızda işlem yapma
5. **İstatistiksel Analiz**: Veri toplama ve değerlendirme

## 🤝 Katkıda Bulunma

Katkılarınızı bekliyoruz! Lütfen:

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Pull Request açın

## 📝 Lisans

Bu proje Apache 2.0 lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## 👨‍💻 Geliştirici

- **Geliştirici**: Deniz ÇAKMAK
- **GitHub**: [@Deniz0648](https://github.com/Deniz0648)
- **Email**: s.deniz.cakmk@gmail.com

## 🙏 Teşekkürler

- .NET Team - Harika TPL (Task Parallel Library) için
- Matematik öğretmenlerim - Olasılık teorisini öğrettiğiniz için
- Topluluk - Geri bildirimler ve katkılar için

## 📖 Kaynaklar

- [Olasılık Teorisi](https://en.wikipedia.org/wiki/Probability_theory)
- [Task Parallel Library](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl)
- [Big-O Notation](https://en.wikipedia.org/wiki/Big_O_notation)
- [Law of Large Numbers](https://en.wikipedia.org/wiki/Law_of_large_numbers)

---

⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!

**Not**: Bu proje eğitim amaçlıdır. Gerçek dünya uygulamalarında kriptografik olarak güvenli random number generator kullanılmalıdır.
