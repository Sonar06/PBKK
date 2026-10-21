# Week 04 - Student Management System (WPF, MVVM, & SQLite)

**Nama**: Khairan Cherokee Musthofa  
**NRP**: 5025241215  
**Mata Kuliah**: PBKK C  

---

## 1. Tujuan
Membangun aplikasi pengelolaan data mahasiswa berbasis C# dan Windows Presentation Foundation (WPF) pada platform .NET 8.0 dengan fokus pada:
- Penerapan pola arsitektur **Model-View-ViewModel (MVVM)** untuk memisahkan komponen antarmuka pengguna (*View*) dari logika bisnis (*ViewModel*) dan data (*Model*).
- Implementasi mekanisme **Data Binding** dua arah (*Two-Way Binding*), **`INotifyPropertyChanged`**, dan **`ObservableCollection`** untuk pembaruan data secara real-time pada antarmuka.
- Pengelolaan persistensi data menggunakan **Repository Pattern** berbasis database relasional (SQLite / ADO.NET) untuk mendukung operasi **CRUD** (*Create, Read, Update, Delete*) serta fitur pencarian (*Search*).
- Pembuatan antarmuka Rich UI yang interaktif, modern, dan responsif menggunakan skema layout deklaratif XAML.

---

## 2. Struktur Berkas yang Dikumpulkan

Berikut adalah berkas kode sumber utama proyek:

```text
StudentManagent/
├── Models/
│   └── Student.cs             # Entitas data Mahasiswa (Model)
├── Data/
│   ├── Database.cs            # Helper inisialisasi & pembuat tabel SQLite
│   └── StudentRepository.cs   # Data Access Layer (DAL) untuk query SQL CRUD & Search
├── ViewModels/
│   ├── RelayCommand.cs        # Implementasi ICommand untuk penanganan aksi tombol
│   └── StudentViewModel.cs    # Pusat logika UI, state management, & kalkulasi statistik
├── App.xaml / App.xaml.cs     # Entry point aplikasi & trigger inisialisasi database
├── MainWindow.xaml            # Desain Rich UI & konfigurasi Data Binding (View)
├── MainWindow.xaml.cs         # Code-behind penentu DataContext
└── StudentManagent.csproj     # Konfigurasi proyek .NET 8 WPF & dependensi SQLite
```

### Penjelasan Peran Setiap Berkas:

1. **`Models/Student.cs`**:
   Representasi objek data murni yang memetakan properti mahasiswa seperti `Id`, `NIM`, `Nama`, `Jurusan`, `Gender`, dan `Email`.

2. **`Data/Database.cs` & `StudentRepository.cs`**:
   `Database.cs` memastikan file database `StudentDB.db` dan tabel `Students` otomatis terbuat saat aplikasi berjalan. `StudentRepository.cs` bertindak sebagai *Data Access Layer* menggunakan `Microsoft.Data.Sqlite` untuk mengeksekusi perintah SQL secara terisolasi.

3. **`ViewModels/RelayCommand.cs` & `StudentViewModel.cs`**:
   `RelayCommand.cs` menghubungkan komponen *Button* pada XAML dengan *method* pada C# tanpa menulis *event handler* di *code-behind*. `StudentViewModel.cs` mengelola kumpulan data `ObservableCollection<Student>`, kalkulasi statistik secara dinamis, serta perintah simpan, hapus, reset, dan cari.

4. **`MainWindow.xaml` & `MainWindow.xaml.cs`**:
   `MainWindow.xaml` mendefinisikan antarmuka visual seperti Form Input, Panel Statistik, Bar Pencarian, dan DataGrid. `MainWindow.xaml.cs` beroperasi secara minimalis hanya untuk menetapkan `DataContext = new StudentViewModel()`.

---

## 3. Konfigurasi Proyek & Database

Aplikasi dikonfigurasikan pada lingkungan .NET 8 WPF dengan dependensi database SQLite melalui berkas `StudentManagent.csproj`:

### Penjelasan Teknis:
- **`Microsoft.Data.Sqlite`**:
  Pustaka ADO.NET ringan yang digunakan untuk mengeksekusi query database lokal tanpa perlu menginstall server database eksternal.
- **Inisialisasi Otomatis (`App.xaml.cs`)**:
  Perintah `Database.Initialize()` dipanggil pada *method* `OnStartup` untuk menjamin ketersediaan tabel database sebelum komponen antarmuka pengguna dirender.

---

## 4. Pusat Kendali Logika dan Arsitektur MVVM

Aplikasi menerapkan pemisahan tanggung jawab (*Separation of Concerns*) berbasis pola arsitektur MVVM:

### 4.1 Data Access Layer & Query SQL (`StudentRepository.cs`)
- **READ (`GetAll`)**: Mengambil seluruh record mahasiswa dari tabel `Students` dengan urutan `Id DESC`.
- **INSERT (`Insert`)**: Menambahkan baris data mahasiswa baru menggunakan *parameterized query* `@NIM`, `@Nama`, dst. untuk mencegah serangan *SQL Injection*.
- **UPDATE (`Update`)**: Memperbarui record mahasiswa berdasarkan atribut `Id`.
- **DELETE (`Delete`)**: Menghapus baris data mahasiswa dari database sesuai `Id` yang dipilih.
- **SEARCH (`Search`)**: Memfilter data mahasiswa menggunakan klausa `LIKE` pada kolom `NIM`, `Nama`, atau `Jurusan`.

### 4.2 State Management & Command (`StudentViewModel.cs`)
- **`ObservableCollection<Student>`**:
  Koleksi dinamis yang secara otomatis memberi tahu UI (*DataGrid*) untuk menambah/merefresh baris tampilan setiap kali terjadi perubahan isi data.
- **`INotifyPropertyChanged`**:
  Antarmuka untuk mengirim notifikasi ke UI agar tampilan properti seperti statistik (`TotalStudents`, `TotalInformatika`, dll) langsung diperbarui tanpa perlu memuat ulang seluruh halaman.
- **`ICommand` Binding**:
  - `SaveCommand`: Mengeksekusi penambahan data baru jika `Id == 0` atau pembaruan jika `Id > 0`.
  - `DeleteCommand`: Menghapuskan data mahasiswa yang sedang terpilih pada `DataGrid`.
  - `ResetCommand`: Mengosongkan isian form input.
  - `SearchCommand`: Memanggil fungsi pencarian berdasarkan teks pada `SearchText`.

---

## 5. Alur Operasi CRUD & Data Binding

Seluruh interaksi pada antarmuka berjalan tanpa *code-behind event handler* (seperti `Click=""`), melainkan memanfaatkan fitur bawaan WPF:

1. **Form Input $\rightarrow$ Model**: `TextBox` dan `ComboBox` terikat langsung (*Two-Way Binding*) ke atribut `SelectedStudent` (contoh: `{Binding SelectedStudent.NIM, UpdateSourceTrigger=PropertyChanged}`).
2. **Selection DataGrid**: Baris yang diklik pada `DataGrid` otomatis mengisi atribut `SelectedStudent`, sehingga data mahasiswa tersebut langsung muncul pada form input untuk siap diedit atau dihapus.
3. **Statistik Real-time**: Metode `RefreshStatistics()` memicu pembaruan `OnPropertyChanged` untuk seluruh properti rekapitulasi data setiap kali operasi CRUD selesai dijalankan.

---

## 6. Dokumentasi & Hasil Pengujian

- **Tampilan Utama & Data Grid**: Tampilan aplikasi menggunakan layout *Rich UI* yang memisahkan Form Input/Statistik di sebelah kiri dan Bar Pencarian serta DataGrid di sebelah kanan.

  <img width="1467" height="912" alt="image" src="https://github.com/user-attachments/assets/7f5944e3-2833-4c8e-90a2-d2b5cd8e130f" />


- **Pengujian Create & Update Data**: Data baru yang diisikan melalui form akan langsung tersimpan ke database SQLite dan memperbarui statistik serta daftar pada DataGrid.

  <img width="1470" height="912" alt="image" src="https://github.com/user-attachments/assets/85027005-f8e8-42f9-afce-fbac0649235a" />
  <br></br>
  saya ada type di nama Musthfoa yang harusnya Musthofa
  <img width="1460" height="906" alt="image" src="https://github.com/user-attachments/assets/409369a4-36f6-4e26-a559-f888fdda826f" />

- **Pengujian Pencarian & Statistik**: Mengisikan kata kunci pada bar pencarian akan memfilter daftar mahasiswa secara spesifik, serta angka statistik rekapitulasi menyesuaikan secara otomatis.

  <img width="1462" height="903" alt="image" src="https://github.com/user-attachments/assets/d42d9090-de38-44b2-830f-8d4a3ea86ae1" />
  <br></br>
   saya ingin mencari nama Khairan
  <img width="1466" height="908" alt="image" src="https://github.com/user-attachments/assets/b122d4d6-3c89-4802-926b-f36ba32bff43" />


[Source Code](https://github.com/Sonar06/PBKK/tree/main/Week-4/StudentManagent)
