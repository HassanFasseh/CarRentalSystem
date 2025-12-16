# Email Configuration Guide

This guide explains how to set up email functionality for the Car Rental System.

## 📧 Email Features

The system sends emails for:
- **Rental Request Confirmation** - When a client submits a rental request
- **Rental Approval** - When an admin approves a rental request
- **Rental Denial** - When an admin denies a rental request

## 🔧 Setup Instructions

### Option 1: Gmail (Recommended for Testing)

1. **Enable 2-Step Verification**
   - Go to your Google Account settings
   - Enable 2-Step Verification if not already enabled

2. **Generate an App Password**
   - Go to: https://myaccount.google.com/apppasswords
   - Select "Mail" and "Other (Custom name)"
   - Enter "Car Rental System" as the name
   - Click "Generate"
   - Copy the 16-character password (you'll use this, not your regular password)

3. **Configure Web Application (MVC)**
   - Edit `CarRental.Web/appsettings.json`
   - Update the EmailSettings section:
   ```json
   {
     "EmailSettings": {
       "SmtpServer": "smtp.gmail.com",
       "SmtpPort": "587",
       "SmtpUsername": "your-email@gmail.com",
       "SmtpPassword": "your-16-character-app-password",
       "FromEmail": "your-email@gmail.com"
     }
   }
   ```

4. **Configure BackOffice (WPF)**
   - Copy `email.config.example` to `email.config` in the BackOffice bin folder
   - Or create `email.config` in: `CarRental.BackOffice/bin/Debug/net8.0-windows/`
   - Edit `email.config`:
   ```
   SmtpServer=smtp.gmail.com
   SmtpPort=587
   SmtpUsername=your-email@gmail.com
   SmtpPassword=your-16-character-app-password
   FromEmail=your-email@gmail.com
   ```

### Option 2: Outlook/Hotmail

1. **Configure Web Application**
   ```json
   {
     "EmailSettings": {
       "SmtpServer": "smtp-mail.outlook.com",
       "SmtpPort": "587",
       "SmtpUsername": "your-email@outlook.com",
       "SmtpPassword": "your-password",
       "FromEmail": "your-email@outlook.com"
     }
   }
   ```

2. **Configure BackOffice**
   ```
   SmtpServer=smtp-mail.outlook.com
   SmtpPort=587
   SmtpUsername=your-email@outlook.com
   SmtpPassword=your-password
   FromEmail=your-email@outlook.com
   ```

### Option 3: Yahoo Mail

1. **Configure Web Application**
   ```json
   {
     "EmailSettings": {
       "SmtpServer": "smtp.mail.yahoo.com",
       "SmtpPort": "587",
       "SmtpUsername": "your-email@yahoo.com",
       "SmtpPassword": "your-app-password",
       "FromEmail": "your-email@yahoo.com"
     }
   }
   ```

2. **Configure BackOffice**
   ```
   SmtpServer=smtp.mail.yahoo.com
   SmtpPort=587
   SmtpUsername=your-email@yahoo.com
   SmtpPassword=your-app-password
   FromEmail=your-email@yahoo.com
   ```

### Option 4: Custom SMTP Server

If you have your own SMTP server, use its settings:

**Web Application:**
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.yourdomain.com",
    "SmtpPort": "587",
    "SmtpUsername": "your-username",
    "SmtpPassword": "your-password",
    "FromEmail": "noreply@yourdomain.com"
  }
}
```

**BackOffice:**
```
SmtpServer=smtp.yourdomain.com
SmtpPort=587
SmtpUsername=your-username
SmtpPassword=your-password
FromEmail=noreply@yourdomain.com
```

## 🧪 Testing Email

### Test from Web Application

1. Register a new client account
2. Request a rental
3. Check the client's email inbox for confirmation

### Test from BackOffice

1. Login to BackOffice as admin
2. Go to Rentals page
3. Select a pending rental
4. Click "Approve" or "Deny"
5. Check the client's email inbox for notification

## ⚠️ Troubleshooting

### Emails Not Sending

1. **Check SMTP Settings**
   - Verify server, port, username, and password are correct
   - For Gmail, make sure you're using an App Password, not your regular password

2. **Check Firewall/Antivirus**
   - Some firewalls block SMTP connections
   - Try temporarily disabling to test

3. **Check Port**
   - Port 587 (TLS) is most common
   - Some providers use port 465 (SSL) or 25

4. **Check Debug Output**
   - In Visual Studio/Rider, check the Debug Output window
   - Look for email-related messages

### Gmail Specific Issues

- **"Less secure app access"** - Gmail no longer supports this. Use App Passwords instead.
- **"Authentication failed"** - Make sure you're using an App Password, not your regular password
- **"Connection timeout"** - Check your internet connection and firewall settings

### BackOffice Email Config Not Found

- Make sure `email.config` is in the same folder as `CarRental.BackOffice.exe`
- Default location: `CarRental.BackOffice/bin/Debug/net8.0-windows/email.config`
- The file should be in the same directory as the executable

## 📝 Notes

- **Security**: Never commit email passwords to version control
- **Development**: If email is not configured, the system will still work but emails won't be sent
- **Production**: Use environment variables or secure configuration management for production
- **Testing**: You can test without real emails - the system will log email attempts to debug output

## 🔒 Security Best Practices

1. **Use App Passwords** - Don't use your main account password
2. **Environment Variables** - For production, use environment variables instead of config files
3. **Separate Email Account** - Consider using a dedicated email account for the application
4. **Regular Updates** - Keep MailKit package updated for security patches

---

**Need Help?** Check the application's debug output for detailed error messages.

