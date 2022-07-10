using System.Net;

public class Server
{

    private static readonly string[] indexFiles =
    {
        "index.html",
        "index.htm",
        "default.html",
        "default.htm"
    };

    // For loading css, js, etc.
    private static IDictionary<string, string> mimeTypes =
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
            {   ".mp4",     "video/mpeg"                            },
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

    private HttpListener? listener;
    private Thread? thread;
    private string path;
    private string ip;
    private int port;

    public Server(int port, string path, string ip)
    {
        this.path = path;
        this.ip = ip;
        this.port = port;
    }

    public void Start()
    {
        if (thread != null) throw new Exception("winserve is already active. (Call stop first)");
        thread = new Thread(Listen);
        thread.Start();
    }

    private void Listen()
    {
        bool threadActive = true;

        if (!HttpListener.IsSupported)
        {
            Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            return;
        }

        try
        {
            listener = new HttpListener();
            string prefix = string.Format("http://{0}:{1}/", ip, port);
            listener.Prefixes.Add(prefix);
            listener.Start();
            Console.WriteLine("Listening...");
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR:" + e.Message);
            threadActive = false;
            return;
        }


        while (threadActive)
        {

            try
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                if (!threadActive) break;

                ProcessContext(context);
            }
            catch (HttpListenerException ex)
            {
                Console.Write(ex);
            }
        }
    }

    private void ProcessContext(HttpListenerContext context)
    {
        string filename = context.Request.Url!.AbsolutePath;


        if (filename is not null) filename = System.Web.HttpUtility.UrlDecode(filename.Substring(1));

        if (string.IsNullOrEmpty(filename))
        {
            foreach (string indexFile in indexFiles)
            {
                if (File.Exists(Path.Combine(path, indexFile)))
                {
                    filename = indexFile;
                    break;
                }
            }
        }

        Console.WriteLine($"Serving file: {filename}");
        filename = Path.Combine(path, filename!);

        HttpStatusCode statusCode;

        if (File.Exists(filename))
        {
            try
            {
                using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    context.Response.ContentType = mimeTypes[Path.GetExtension(filename)];
                    context.Response.ContentLength64 = stream.Length;

                    // copy file stream to response
                    stream.CopyTo(context.Response.OutputStream);
                    stream.Flush();
                    context.Response.OutputStream.Flush();
                }

                statusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                statusCode = HttpStatusCode.InternalServerError;
            }
        }
        else
        {
            Console.WriteLine("File not found: " + filename);
            statusCode = HttpStatusCode.NotFound;
        }

        context.Response.StatusCode = (int)statusCode;
        if (statusCode == HttpStatusCode.OK)
        {
            context.Response.AddHeader("Date", DateTime.Now.ToString("R"));
            context.Response.AddHeader("Last-Modified", File.GetLastWriteTime(filename).ToString("R"));
        }
        context.Response.OutputStream.Close();
    }
}
