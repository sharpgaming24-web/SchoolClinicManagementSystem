# City College of Tagaytay - School Clinic Management System (SCMS)

A complete **C# .NET Framework (WinForms)** desktop application featuring the pixel-perfect **SCMS Portal Login** and a **Clinic Pharmacy & Medicine Management Dashboard** with an auto-collapsible / shrinkable overview panel.

---

## 🌟 Key Features

### 1. 🔐 Pixel-Perfect Login Screen (`LoginForm`)
Recreates the exact visual design from the City College of Tagaytay reference:
- **Title Bar**: `Login - School Clinic Management System` with official CCT seal icon.
- **Top Maroon Banner** (`#240C12`): Official City College of Tagaytay crest, `SCMS PORTAL` title, and subtitle.
- **Form Card**:
  - `Welcome Back` title & description.
  - `ModernTextBox` with blue outline focus ring (`#0D6EFD`) on focus.
  - Masked password characters.
  - Crimson `Log In` button (`#880E1E`).
  - Default credentials hint: `Default: admin / admin123 • nurse / nurse123`.

---

### 2. 📊 Dynamic Collapsible Dashboard Overview
The **Dashboard Overview** at the top of the clinic workspace features intelligent auto-shrinking behavior:
- **Hover to Expand**: When the overview is collapsed, simply moving the mouse over the bar automatically expands it to display full KPI statistics and quick actions.
- **Auto-Shrink on Mouse Leave**: When the mouse leaves the overview panel (not hovered), it automatically collapses down into a compact 40px summary bar (`8 Medicines • 349 Units on hand • 2 Low Stock Alerts • 4 Dispensed Today`).
- **Button Controls to Show/Hide/Pin**:
  - **`[▼ Show Overview]` / `[▲ Shrink Overview]`**: Click to toggle overview expansion at any time.
  - **`[📊 Overview]`** button in the top navigation header to restore/collapse from anywhere.
  - **`[📌 Pin Overview]`**: Locks the overview in the expanded state so it won't auto-shrink until unpinned.

#### Overview Metrics:
- 💊 **Total Medicines**: Count of distinct registered formulary items.
- 📦 **Stock on Hand**: Aggregated units across all medicines and supplies.
- ⚠️ **Low Stock Alerts**: Count of items requiring restock (below reorder threshold).
- 📋 **Dispensed Today**: Total medicine units issued to students & staff today.

---

### 3. 💊 Clinic Pharmacy & Medicine Inventory
- **Real-Time Search & Filtering**: Instant search by Brand Name, Generic Name, or Category; filter by *All*, *Low Stock / Out of Stock*, or *In Stock*.
- **Add / Edit Medicine**: Manage brand, generic, category, dosage form, stock quantity, units, reorder alert level, and expiration date.
- **Dispense Medicine**: Issue medicine directly to a student or staff member with quantity validation, automatic stock deduction, and reason logging.
- **Restock Medicine**: Add incoming shipment quantities and update expiration dates.
- **Dispense Audit Log**: Track all historical medicine disbursements with recipient names, roles, courses, timestamps, and attending nurse signatures.

---

## 🔑 Default Login Credentials

| Role | Username | Password | Full Name |
| :--- | :--- | :--- | :--- |
| **Administrator** | `admin` | `admin123` | System Administrator |
| **School Nurse** | `nurse` | `nurse123` | Maria Santos, RN (Clinic Nurse) |

---

## 🚀 How to Run and Build

### Quick Run with `dotnet` CLI:
```powershell
dotnet run
```

### Direct Executable:
Double-click:
```
bin\Debug\net48\SchoolClinicManagementSystem.exe
```

### Visual Studio:
Open [`SchoolClinicManagementSystem.sln`](file:///c:/Users/Francis/AntiGravity/SchoolClinicManagementSystem.sln) in **Visual Studio 2019 / 2022 / 2026** and press **F5**.
