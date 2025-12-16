# Pushing Project to GitHub - Step by Step Guide

This guide will help you push your Car Rental System project to GitHub.

## 📋 Prerequisites

1. **Git installed on your computer**
   - Download: [Git for Windows](https://git-scm.com/download/win)
   - During installation, choose "Git from the command line and also from 3rd-party software"
   - After installation, restart your terminal/command prompt

2. **GitHub account**
   - Create one at: [GitHub](https://github.com/)
   - Verify your email address

## 🚀 Step-by-Step Instructions

### Step 1: Install Git (if not already installed)

1. **Download Git:**
   - Go to: https://git-scm.com/download/win
   - Download the installer
   - Run the installer

2. **Installation Options:**
   - Use default options (recommended)
   - Make sure "Git from the command line and also from 3rd-party software" is selected
   - Click "Next" through the installation

3. **Verify Installation:**
   - Open a **new** PowerShell or Command Prompt window
   - Run: `git --version`
   - You should see something like: `git version 2.x.x`

### Step 2: Configure Git (First Time Only)

If this is your first time using Git, configure your name and email:

```bash
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

**Example:**
```bash
git config --global user.name "Hassan"
git config --global user.email "fassehhassan001@gmail.com"
```

### Step 3: Navigate to Your Project Folder

Open PowerShell or Command Prompt and navigate to your project:

```bash
cd "C:\Users\hassa\OneDrive\Desktop\Projet NET"
```

### Step 4: Initialize Git Repository

Initialize a new Git repository in your project folder:

```bash
git init
```

This creates a hidden `.git` folder in your project.

### Step 5: Add All Files to Git

Add all project files to Git (the `.gitignore` file will automatically exclude build files and sensitive configs):

```bash
git add .
```

**Note:** This will add all files except those listed in `.gitignore` (like `email.config` with passwords).

### Step 6: Create Initial Commit

Create your first commit with all the project files:

```bash
git commit -m "Initial commit: Car Rental System project"
```

### Step 7: Create a New Repository on GitHub

1. **Go to GitHub:**
   - Open: https://github.com/
   - Sign in to your account

2. **Create New Repository:**
   - Click the **+** icon in the top right
   - Select **New repository**

3. **Repository Settings:**
   - **Repository name:** `CarRentalSystem` (or any name you prefer)
   - **Description:** "Academic Car Rental Management System - .NET 8, WPF, ASP.NET Core MVC"
   - **Visibility:** Choose **Public** (for portfolio) or **Private** (if you prefer)
   - **DO NOT** check "Initialize this repository with a README" (we already have files)
   - **DO NOT** add .gitignore or license (we already have them)
   - Click **Create repository**

4. **Copy the Repository URL:**
   - GitHub will show you a page with instructions
   - Copy the **HTTPS** URL (looks like: `https://github.com/yourusername/CarRentalSystem.git`)
   - Or copy the **SSH** URL if you've set up SSH keys

### Step 8: Connect Local Repository to GitHub

Add GitHub as a remote repository:

```bash
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git
```

**Replace with your actual GitHub username and repository name.**

**Example:**
```bash
git remote add origin https://github.com/hassan/CarRentalSystem.git
```

### Step 9: Push to GitHub

Push your code to GitHub:

```bash
git branch -M main
git push -u origin main
```

**If prompted for credentials:**
- **Username:** Your GitHub username
- **Password:** Use a **Personal Access Token** (not your GitHub password)
  - See "Creating a Personal Access Token" below if needed

### Step 10: Verify on GitHub

1. **Refresh your GitHub repository page**
2. **You should see all your project files**
3. **Check that sensitive files are NOT uploaded:**
   - `email.config` should NOT be visible (it's in `.gitignore`)
   - `appsettings.Development.json` should NOT be visible

## 🔐 Creating a Personal Access Token (for GitHub Password)

GitHub no longer accepts passwords for Git operations. You need a Personal Access Token:

### Steps:

1. **Go to GitHub Settings:**
   - Click your profile picture (top right)
   - Click **Settings**

2. **Navigate to Developer Settings:**
   - Scroll down to **Developer settings** (bottom left)

3. **Create Personal Access Token:**
   - Click **Personal access tokens**
   - Click **Tokens (classic)**
   - Click **Generate new token** → **Generate new token (classic)**

4. **Configure Token:**
   - **Note:** "CarRental Project"
   - **Expiration:** Choose duration (90 days, or custom)
   - **Scopes:** Check **repo** (full control of private repositories)
   - Click **Generate token**

5. **Copy the Token:**
   - **IMPORTANT:** Copy the token immediately (you won't see it again!)
   - It looks like: `ghp_xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx`

6. **Use Token as Password:**
   - When Git asks for password, paste the token instead

## 📝 Quick Command Reference

Here are all the commands in one place (run these in order):

```bash
# Navigate to project
cd "C:\Users\hassa\OneDrive\Desktop\Projet NET"

# Initialize Git
git init

# Configure Git (first time only)
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"

# Add all files
git add .

# Create commit
git commit -m "Initial commit: Car Rental System project"

# Add remote repository (replace with your GitHub URL)
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git

# Push to GitHub
git branch -M main
git push -u origin main
```

## 🔄 Updating Your Repository (Future Changes)

After making changes to your project:

```bash
# Navigate to project folder
cd "C:\Users\hassa\OneDrive\Desktop\Projet NET"

# Check what changed
git status

# Add changed files
git add .

# Commit changes
git commit -m "Description of what you changed"

# Push to GitHub
git push
```

## ⚠️ Important Notes

### Files NOT Uploaded (Protected by .gitignore)

The following files are **NOT** uploaded to GitHub (they contain sensitive information):

- ✅ `CarRental.BackOffice/email.config` - Contains email passwords
- ✅ `CarRental.Web/appsettings.Development.json` - May contain sensitive config
- ✅ All `bin/` and `obj/` folders - Build outputs
- ✅ `.vs/` and `.idea/` folders - IDE settings

### Files That ARE Uploaded

- ✅ All source code (`.cs` files)
- ✅ All XAML and Razor views
- ✅ `README.md`, `VISUAL_STUDIO_SETUP.md`, `EMAIL_SETUP.md`
- ✅ `email.config.example` (template without passwords)
- ✅ `appsettings.json` (without sensitive data)
- ✅ Database migrations
- ✅ Project files (`.csproj`, `.sln`)

## 🐛 Troubleshooting

### "git is not recognized"

**Problem:** Git command not found

**Solution:**
1. Install Git from: https://git-scm.com/download/win
2. Restart your terminal/PowerShell after installation
3. Verify with: `git --version`

### "fatal: not a git repository"

**Problem:** You're not in a Git repository

**Solution:**
```bash
cd "C:\Users\hassa\OneDrive\Desktop\Projet NET"
git init
```

### "remote origin already exists"

**Problem:** You already added the remote

**Solution:**
```bash
# Remove existing remote
git remote remove origin

# Add it again with correct URL
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git
```

### "Authentication failed" or "Invalid credentials"

**Problem:** GitHub password authentication doesn't work

**Solution:**
- Use a **Personal Access Token** instead of password
- See "Creating a Personal Access Token" section above

### "Permission denied (publickey)"

**Problem:** SSH key not set up (if using SSH URL)

**Solution:**
- Use HTTPS URL instead: `https://github.com/username/repo.git`
- Or set up SSH keys (more complex)

### "Failed to push some refs"

**Problem:** Remote repository has changes you don't have locally

**Solution:**
```bash
# Pull changes first
git pull origin main --allow-unrelated-histories

# Then push
git push
```

## 📚 Additional Resources

- **Git Documentation:** https://git-scm.com/doc
- **GitHub Guides:** https://guides.github.com/
- **Git Cheat Sheet:** https://education.github.com/git-cheat-sheet-education.pdf

## ✅ Checklist

Before pushing, make sure:

- [ ] Git is installed and working (`git --version`)
- [ ] Git is configured with your name and email
- [ ] `.gitignore` file exists in project root
- [ ] Sensitive files (email.config) are NOT in the repository
- [ ] All source code files are present
- [ ] GitHub repository is created
- [ ] Personal Access Token is created (if needed)
- [ ] Remote repository is added correctly
- [ ] Code is committed locally
- [ ] Code is pushed to GitHub successfully

---

**Good luck with your GitHub upload! 🚀**

If you encounter any issues, refer to the troubleshooting section or check the Git documentation.

