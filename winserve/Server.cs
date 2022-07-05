using System.Net;

internal class Server
{
    // For loading css, js, etc.
    private static IDictionary<string, string> mimeTypeMappings =
        new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            #region extension to MIME type list
            {   ".asf",     "video/x-ms-asf"                        },
            {   ".asx",     "video/x-ms-asx"                        },
            {   ".avi",     "video/x-msvideo"                       },
            {   ".bin",     "application/octet-stream"              },
            {   ".cco",     "application/cocoa"                     },
            {   ".crt",     "application/x-x509-ca-cert"            },
            {   ".css",     "text/css"                              },
            {   ".deb",     "application/octet-stream"              },
            {   ".der",     "application/x-x509-ca-cert"            },
            {   ".dll",     "application/octet-stream"              },
            {   ".dmg",     "application/octet-stream"              },
            {   ".ear",     "application/java-archive"              },
            {   ".eot",     "application/octet-stream"              },
            {   ".exe",     "application/octet-stream"              },
            {   ".flv",     "video/x-flv"                           },
            {   ".gif",     "image/gif"                             },
            {   ".hqx",     "applicaiton/mac-binhex40"              },
            {   ".htc",     "text/x-component"                      },
            {   ".htm",     "text/html"                             },
            {   ".html",    "text/html"                             },
            {   ".ico",     "image/x-icon"                          },
            {   ".jar",     "application/java-archive"              },
            {   ".jardiff", "application/x-java-archive-diff"       },
            {   ".jng",     "image/x-jng"                           },
            {   ".jnlp",    "application/x-java-jnlp-file"          },
            {   ".jpeg",    "image/jpeg"                            },
            {   ".jpg",     "image/jpeg"                            },
            {   ".js",      "application/x-javascript"              },
            {   ".mml",     "text/mathml"                           },
            {   ".mng",     "video/x-mng"                           },
            {   ".mov",     "video/quicktime"                       },
            {   ".mp3",     "audio/mpeg"                            },
            {   ".mpeg",    "video/mpeg"                            },
            {   ".mpg",     "video/mpeg"                            },
            {   ".msi",     "application/octet-stream"              },
            {   ".msm",     "application/octet-stream"              },
            {   ".msp",     "application/octet-stream"              },
            {   ".pdb",     "application/x-pilot"                   },
            {   ".pdf",     "application/pdf"                       },
            {   ".pem",     "application/x-x509-ca-cert"            },
            {   ".pl",      "application/x-perl"                    },
            {   ".pm",      "application/x-perl"                    },
            {   ".png",     "image/png"                             },
            {   ".prc",     "application/x-pilot"                   },
            {   ".ra",      "audio/x-realaudio"                     },
            {   ".rar",     "application/x-rar-compressed"          },
            {   ".rpm",     "application/x-redhat-package-manager"  },
            {   ".rss",     "text/xml"                              },
            {   ".run",     "application/x-makeself"                },
            {   ".sea",     "application/x-sea"                     },
            {   ".shtml",   "text/html"                             },
            {   ".sit",     "application/x-stuffit"                 },
            {   ".swf",     "application/x-shockwave-flash"         },
            {   ".tcl",     "application/x-tcl"                     },
            {   ".tk",      "application/x-tcl"                     },
            {   ".txt",     "text/plain"                            },
            {   ".war",     "application/java-archive"              },
            {   ".wbmp",    "image/vnd.wap.wbmp"                    },
            {   ".wmv",     "video/x-ms-wmv"                        },
            {   ".xml",     "text/xml"                              },
            {   ".xpi",     "application/x-xpinstall"               },
            {   ".zip",     "application/zip"                       },
            #endregion
        };
    internal void Start(string[] prefixes)
    {
        HttpListener httpListener = new HttpListener();

        if (!HttpListener.IsSupported)
        {
            Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            return;
        }

        if (prefixes is null || prefixes.Length == 0)
        {
            throw new ArgumentException("prefixes");

        }
        foreach (string s in prefixes)
        {
            httpListener.Prefixes.Add(s);
        }

        httpListener.Start();
        Console.WriteLine("Listening...");
        // Note: The GetContext method blocks while waiting for a request.
        HttpListenerContext context = httpListener.GetContext();
        HttpListenerRequest request = context.Request;
        // Obtain a response object.
        HttpListenerResponse response = context.Response;
        // Construct a response.
        string responseString = GetIndexHTMLFile();
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // You must close the output stream.
        output.Close();
        httpListener.Stop();
    }

    internal string GetIndexHTMLFile()
    {
        string path = Directory.GetCurrentDirectory();

        try
        {
            var htmlFilesInDirectory = Directory.EnumerateFiles(path, "*.html", SearchOption.AllDirectories);

            foreach (string currentFile in htmlFilesInDirectory)
            {
                string content = File.ReadAllText(currentFile);
                return content;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        // This shouldn't return an empty string, I should really find a better way to do this
        return "";
    }
}
