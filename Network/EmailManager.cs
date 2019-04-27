// crudwork
// Copyright 2004 by Steve T. Pham (http://www.crudwork.com)
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with This program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.IO;
using crudwork.Utilities;

namespace crudwork.Network
{
	/// <summary>
	/// Email Manager
	/// </summary>
	public class EmailManager
	{
		private SmtpClient smtp;

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="host"></param>
		public EmailManager(string host)
			: this(host, 25)
		{
		}

		/// <summary>
		/// Create new instance with given attributes
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		public EmailManager(string host, int port)
		{
			smtp = new SmtpClient(host, port);
			smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
		}

		/// <summary>
		/// Send an email
		/// </summary>
		/// <param name="from">specify the sender's email address</param>
		/// <param name="to">specify the to email address list (separated by comma or semicolon)</param>
		/// <param name="subject">the email subject</param>
		/// <param name="body">the email body</param>
		/// <param name="attachments">list of filename attachments</param>
		public void Send(string from, string to, string subject, string body, params string[] attachments)
		{
			Send(from, to, null, null, subject, body, attachments);
		}

		/// <summary>
		/// Send an email
		/// </summary>
		/// <param name="from">specify the sender's email address</param>
		/// <param name="to">specify the to email address list (separated by comma or semicolon)</param>
		/// <param name="cc">specify the cc email address list (separated by comma or semicolon)</param>
		/// <param name="bcc">specify the bcc email address list (separated by comma or semicolon)</param>
		/// <param name="subject">the email subject</param>
		/// <param name="body">the email body</param>
		/// <param name="attachments">list of filename attachments</param>
		public void Send(string from, string to, string cc, string bcc, string subject, string body, params string[] attachments)
		{
			try
			{
				if (string.IsNullOrEmpty(from))
					throw new ArgumentNullException("from");
				if (string.IsNullOrEmpty(to))
					throw new ArgumentNullException("to");

				string[] tokens;

				using (var message = new MailMessage())
				{
					message.From = new MailAddress(from);

					#region Add TO, CC and BCC
					tokens = to.Split(';', ',');
					for (int i = 0; i < tokens.Length; i++)
					{
						message.To.Add(new MailAddress(tokens[i].Trim(' ', '\t')));
					}

					if (!string.IsNullOrEmpty(cc))
					{
						tokens = cc.Split(';', ',');
						for (int i = 0; i < tokens.Length; i++)
						{
							message.CC.Add(new MailAddress(tokens[i].Trim(' ', '\t')));
						}
					}
					if (!string.IsNullOrEmpty(bcc))
					{
						tokens = bcc.Split(';', ',');
						for (int i = 0; i < tokens.Length; i++)
						{
							message.Bcc.Add(new MailAddress(tokens[i].Trim(' ', '\t')));
						}
					}
					#endregion

					message.Subject = subject;
					message.Body = body;

					#region Add Attachments
					if (attachments != null)
					{
						for (int i = 0; i < attachments.Length; i++)
						{
							string filename = attachments[i];
							if (!File.Exists(filename))
								throw new FileNotFoundException("File not found: " + filename);
							Attachment att = new Attachment(filename, MediaTypeNames.Application.Octet);
							message.Attachments.Add(att);
						}
					}
					#endregion

					smtp.Send(message);
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "senders", to);
				DebuggerTool.AddData(ex, "from", from);
				DebuggerTool.AddData(ex, "subject", subject);
				DebuggerTool.AddData(ex, "body", body);
				DebuggerTool.AddData(ex, "attachments", attachments);
				throw;
			}
		}
	}
}
