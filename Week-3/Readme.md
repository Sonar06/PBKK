# Week 03 - Membuat Aplikasi Kalkulator Scientific (WPF)

**Nama**: Khairan Cherokee Musthofa  
**NRP**: 5025241215
**Mata Kuliah**: PBKK C  
 

---

## 1. Tujuan
Membangun aplikasi kalkulator berbasis C# dan Windows Presentation Foundation (WPF) pada platform .NET 8.0 dengan fokus pada:
- Pengelolaan alur program dan interaksi pengguna melalui sistem kendali *event-driven*.
- Penerapan konsep Object-Oriented Programming (OOP) dengan memisahkan logika komputasi matematika dan pengendali antarmuka (*Separation of Concerns*).
- Implementasi logika aritmatika standar serta fitur lanjutan: operasi **Pangkat ($x^y$)** dan **Akar Kuadrat ($\sqrt{x}$)**.
- Penerapan mekanisme penanganan kesalahan (*exception handling*) untuk mencegah terjadinya penghentian paksa (*crash*) pada program.

---

## 2. Struktur Berkas yang Dikumpulkan

Berikut adalah berkas kode sumber utama proyek:

```text
KalkulatorWPF/
├── KalkulatorWPF.csproj    # Berkas konfigurasi proyek .NET 8 WPF
├── CalculatorEngine.cs     # Class logika perhitungan matematika murni (OOP Engine)
├── MainWindow.xaml         # Desain tata letak antarmuka grafis (XAML)
└── MainWindow.xaml.cs      # Pengendali logika tampilan, event handling, & input keyboard
```

### Penjelasan Peran Setiap Berkas:

1. **`CalculatorEngine.cs`**:
   Class murni (*Pure C# Class*) yang bertindak sebagai mesin komputasi tanpa ketergantungan visual. Berkas ini bertanggung jawab mengolah perhitungan aritmatika (`+`, `-`, `*`, `/`), operasi pangkat (`^`), operasi akar kuadrat (`√`), serta memvalidasi aturan matematika.

2. **`MainWindow.xaml`**:
   Berkas deklaratif berbasis XAML (*Extensible Application Markup Language*) yang mengatur skema tata letak (*Grid*), tampilan layar (*TextBox* & *TextBlock*), serta tombol-tombol interaktif kalkulator.

3. **`MainWindow.xaml.cs`**:
   Berkas pengendali (*Code-Behind*) yang menangani seluruh logika interaksi program, mulai dari manajemen *state* kalkulator, penanganan event klik tombol (*event handler*), eksekusi instan operasi akar, pemrosesan tombol keyboard fisik, hingga penanganan error di tingkat antarmuka.

4. **`KalkulatorWPF.csproj`**:
   Berkas XML berbasis MSBuild yang mendefinisikan target framework (`net8.0-windows`), tipe output eksekusi (`WinExe`), serta mengaktifkan pustaka antarmuka grafis bawaan WPF (`<UseWPF>true</UseWPF>`).

---

## 3. Konfigurasi Proyek & Entry Point

Aplikasi memanfaatkan dukungan native WPF pada .NET 8 Windows yang dikonfigurasikan dalam berkas `KalkulatorWPF.csproj`:

### Penjelasan Teknis:
- **`WinExe`**:
  Menginstruksikan kompilator .NET agar menghasilkan berkas eksekusi berbasis GUI tanpa memunculkan jendela terminal/console tambahan saat program dijalankan.
- **`UseWPF`**:
  Mengaktifkan integrasi pustaka antarmuka grafis Windows Presentation Foundation pada lingkungan runtime aplikasi.

---

## 4. Pusat Kendali Logika dan Interaksi: CalculatorEngine.cs & MainWindow.xaml.cs

Sistem dirancang dengan memisahkan modul komputasi matematika dengan pengendali antarmuka pengguna:

### 4.1 Logika Perhitungan Matematika (`CalculatorEngine.cs`)
- **Operasi Biner (`PerformOperation`)**:
  Menjalankan operasi dua operand menggunakan struktur percabangan `switch`:
  - Penjumlahan (`+`), Pengurangan (`-`), Perkalian (`*`), dan Pembagian (`/`).
  - **Operasi Pangkat (`^`)**: Dieksekusi menggunakan pustaka bawaan `Math.Pow(operand1, operand2)`. Dilengkapi pengecekan khusus jika basis bernilai 0 dengan pangkat negatif ($0^{-n}$) untuk melempar `DivideByZeroException`.
  - Pengecekan nilai tak terdefinisi (`double.IsNaN`) dan nilai melampaui batas (`double.IsInfinity`).
- **Operasi Unari (`PerformUnaryOperation`)**:
  - **Operasi Akar Kuadrat (`√`)**: Dieksekusi menggunakan pustaka bawaan `Math.Sqrt(operand)`. Dilengkapi validasi kondisi (`operand < 0`) untuk menolak akar bilangan negatif dengan melempar `ArgumentException`.

### 4.2 Pengendali Tampilan & Event Handler (`MainWindow.xaml.cs`)
- **Manajemen Status (*State Management*)**:
  Menyimpan variabel `_firstValue`, `_currentOperator`, serta penanda status (`_isOperatorClicked` dan `_isResultShown`) untuk mendukung alur operasi bertahap (*chaining*) tanpa harus menekan tombol sama dengan di setiap tahap.
- **Kalkulasi Akar Cepat (`ProcessRoot`)**:
  Mengeksekusi akar kuadrat secara instan pada nilai yang aktif di layar. Jika ditekan di tengah operasi berjalan, nilai input saat itu langsung dikalkulasi dan histori diperbarui secara transparan.
- **Dukungan Keyboard Fisik (`Window_KeyDown`)**:
  Menangkap event penekanan keyboard:
  - Angka `0`-`9` dan Numpad `0`-`9`.
  - Operator aritmatika (`+`, `-`, `*`, `/`).
  - Pangkat: Kombinasi tombol <kbd>Shift</kbd> + <kbd>6</kbd> (`^`).
  - Akar: Tombol <kbd>R</kbd> (*Root*).
  - Eksekusi: <kbd>Enter</kbd> (sama dengan).
  - Hapus: <kbd>Backspace</kbd> (hapus digit terakhir) dan <kbd>Escape</kbd> / <kbd>C</kbd> (reset total).

---

## 5. Penanganan Kesalahan (Exception Handling)

Implementasi blok `try-catch` diterapkan pada `MainWindow.xaml.cs` saat memanggil metode dari `CalculatorEngine.cs` untuk menangkal penghentian aplikasi secara paksa (*crash*):

- **`DivideByZeroException`**:
  Ditangkap secara spesifik saat terjadi pembagian dengan angka nol atau pangkat $0^{-n}$. Aplikasi menampilkan pesan peringatan `"Tidak dapat membagi dengan nol."` pada label operasi serta mereset tampilan secara aman.
- **`ArgumentException`**:
  Ditangkap ketika pengguna mencoba menghitung akar dari bilangan negatif, memunculkan notifikasi `"Tidak dapat menghitung akar dari bilangan negatif."`.
- **`OverflowException`**:
  Menangani kasus hasil komputasi yang melampaui batas representasi numerik tipe data `double`.
- **`FormatException`**:
  Mencegah kegagalan parsing nilai teks menjadi tipe data numerik.

---

## 6. Dokumentasi & Hasil Pengujian

- **Tampilan Awal**: Aplikasi berada dalam kondisi bersih menampilkan angka default `0` dengan label operasi kosong, siap menerima input dari klik mouse maupun keyboard.
- **Tampilan Operasi Berjalan**: Label operasi menampilkan histori perhitungan secara transparan (contoh: `2 ^ 8 =` atau `10 + √(9)`).
- **Tampilan Penanganan Error**: Layar menampilkan kata `Error` dan label histori menginformasikan jenis pesan kesalahan yang terjadi tanpa menghentikan aplikasi.