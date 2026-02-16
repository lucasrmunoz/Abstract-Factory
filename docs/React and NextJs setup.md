# Setting Up React with Next.js on Windows

A complete guide for installing Chocolatey, Node.js, and creating a Next.js React application.

---

## Prerequisites

- Windows 10 or Windows 11
- Administrator access to PowerShell

---

## Step 1: Install Chocolatey Package Manager

Chocolatey is a package manager for Windows that simplifies software installation.

### 1.1 Check if Chocolatey is Already Installed

Open PowerShell and run:

```powershell
choco --version
```

If you see a version number (e.g., `2.2.2`), skip to Step 2. If you get an error, continue below.

### 1.2 Install Chocolatey

Open PowerShell **as Administrator** (right-click > Run as Administrator) and run:

```powershell
Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
```

### 1.3 Verify Chocolatey Installation

Close PowerShell completely, reopen it as Administrator, then run:

```powershell
choco --version
```

Expected output: `2.2.2` or similar version number.

---

## Step 2: Install Node.js via Chocolatey

Node.js is required to run React and Next.js applications.

### 2.1 Install Node.js LTS

In Administrator PowerShell, run:

```powershell
choco install nodejs-lts -y
```

### 2.2 Refresh Environment Variables

After installation, run:

```powershell
refreshenv
```

Or close and reopen PowerShell.

### 2.3 Verify Node.js Installation

Run these commands to verify:

```powershell
node --version
```

Expected output: `v20.x.x` or higher (LTS version).

```powershell
npm --version
```

Expected output: `10.x.x` or higher.

```powershell
npx --version
```

Expected output: `10.x.x` or higher.

---

## Step 3: Install Git (Optional but Recommended)

Git is useful for version control and is required by many npm packages.

### 3.1 Install Git via Chocolatey

```powershell
choco install git -y
```

### 3.2 Refresh and Verify

```powershell
refreshenv
git --version
```

Expected output: `git version 2.x.x`.

---

## Step 4: Create a Next.js Application

Next.js is a React framework that provides routing, server-side rendering, and more.

### 4.1 Navigate to Your Project Directory

```powershell
cd "C:\path\to\your\project"
```

### 4.2 Create the Next.js App

Run the create-next-app command:

```powershell
npx create-next-app@latest my-app
```

Replace `my-app` with your desired project name.

### 4.3 Answer the Setup Prompts

You will be prompted with configuration options:

| Prompt | Recommended Answer |
| ------ | ------------------ |
| Would you like to use TypeScript? | Yes |
| Would you like to use ESLint? | Yes |
| Would you like to use Tailwind CSS? | Yes |
| Would you like your code inside a `src/` directory? | Yes |
| Would you like to use App Router? (recommended) | Yes |
| Would you like to use Turbopack for `next dev`? | Yes |
| Would you like to customize the import alias? | No |

### 4.4 Alternative: Create with All Options in One Command

To skip the prompts entirely:

```powershell
npx create-next-app@latest my-app --typescript --tailwind --eslint --app --src-dir --turbopack --import-alias "@/*"
```

---

## Step 5: Run Your Next.js Application

### 5.1 Navigate to the Project Folder

```powershell
cd my-app
```

### 5.2 Start the Development Server

```powershell
npm run dev
```

### 5.3 Open in Browser

Navigate to: <http://localhost:3000>

You should see the default Next.js welcome page.

---

## Step 6: Verify the Build

Stop the development server with `Ctrl+C`, then run:

### 6.1 Build for Production

```powershell
npm run build
```

This compiles your application and checks for errors.

### 6.2 Run Production Build Locally

```powershell
npm run start
```

---

## Essential Commands Reference

### Chocolatey Commands

| Command | Description |
| ------- | ----------- |
| `choco --version` | Check Chocolatey version |
| `choco install <package> -y` | Install a package |
| `choco upgrade <package> -y` | Upgrade a package |
| `choco upgrade all -y` | Upgrade all packages |
| `choco list --local-only` | List installed packages |
| `choco uninstall <package> -y` | Uninstall a package |

### Node.js and npm Commands

| Command | Description |
| ------- | ----------- |
| `node --version` | Check Node.js version |
| `npm --version` | Check npm version |
| `npm install` | Install project dependencies |
| `npm install <package>` | Install a specific package |
| `npm install -D <package>` | Install as dev dependency |
| `npm uninstall <package>` | Remove a package |
| `npm list` | List installed packages |
| `npm outdated` | Check for outdated packages |
| `npm update` | Update packages |

### Next.js Commands

| Command | Description |
| ------- | ----------- |
| `npm run dev` | Start development server with hot reload |
| `npm run build` | Build for production |
| `npm run start` | Start production server |
| `npm run lint` | Run ESLint to check for issues |

---

## Project Structure After Setup

```text
my-app/
├── src/
│   └── app/
│       ├── layout.tsx      # Root layout component
│       ├── page.tsx        # Home page component
│       ├── globals.css     # Global styles
│       └── favicon.ico     # Site favicon
├── public/                 # Static files (images, etc.)
├── node_modules/           # Installed dependencies
├── package.json            # Project dependencies and scripts
├── package-lock.json       # Locked dependency versions
├── tsconfig.json           # TypeScript configuration
├── tailwind.config.ts      # Tailwind CSS configuration
├── postcss.config.mjs      # PostCSS configuration
├── next.config.ts          # Next.js configuration
├── eslint.config.mjs       # ESLint configuration
└── README.md               # Project readme
```

---

## Troubleshooting

### "choco is not recognized"

- Ensure you installed Chocolatey as Administrator
- Close and reopen PowerShell after installation
- Check if `C:\ProgramData\chocolatey\bin` is in your PATH

### "node is not recognized"

- Run `refreshenv` or restart PowerShell
- Reinstall Node.js: `choco install nodejs-lts -y --force`

### "npm install" fails with permission errors

- Run PowerShell as Administrator
- Or use: `npm config set prefix "C:\Users\<username>\npm-global"`

### Port 3000 already in use

- Use a different port: `npm run dev -- -p 3001`
- Or kill the process using port 3000

---

## Next Steps

After completing this setup, you can:

1. Edit `src/app/page.tsx` to modify the home page
2. Create new pages by adding folders in `src/app/`
3. Add components in a `src/components/` folder
4. Install additional packages with `npm install <package-name>`
5. Learn more at <https://nextjs.org/docs>
