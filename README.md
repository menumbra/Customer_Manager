Customer Manager

A WinUI 3 desktop app using .NET 8 and C# 13, structured with MVVM. It stores customer data in SQLite, supports user/admin login, auto-tags records by date/editor, and creates folders per customer on a NAS.

📋 Features

  🔒 Login system with role-based access (admin & user)
  📁 Customer info entry (Name, Email) with auto-tagging by month, date, and editor
  🗃️ SQLite database: each user gets a local customer.db, while a master main.db stores all entries centrally
  🗂️ Folder creation for each customer inside the NAS structure
  📆 Dynamic folder structure using current Month/Date

🛠️ Technologies Used

      WinUI 3 for UI
      .NET 8 for application logic
      C# 13 for development
      SQLite for database

🧪 How to Run

  ✅ Prerequisites
  
      Windows 10/11
      Visual Studio 2022 (with WinUI and .NET 8 workloads installed)
      Windows App SDK runtime installed

🚀 Steps

    1. Clone the repository:
    2. git clone https://github.com/<your-username>/customer-manager.git
    3. Open the solution in Visual Studio.
    4. Set the project as startup.
    5. Press Ctrl + F5 to run without debugging.

💾 Folder & Database Structure

      E:\TestFolders\
      ├── May\
      │    └── 21\
      │       ├── main.db
      │       └── user1\
      │           ├── customer.db
      │           └── John Doe\

    main.db - central record of all entries
    customer.db - local DB for each editor
    John Doe - folder automatically created for customer-specific files

✅ Login Roles
    admin – full access
    user – restricted to adding customers

📸 Screenshots

Add your screenshots here for login, main UI, folder structure, etc.

📚 Learning Notes

    This project is also part of a personal journey in learning:
    Desktop application development with modern .NET
    Git & GitHub for version control and documentation
    Real-world app architecture (MVVM)

🔗 License

MIT License

Feel free to contribute or fork!
Developed with ❤️ by Jojo Barria

