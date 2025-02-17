﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t4tea.service.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);

    }
}
