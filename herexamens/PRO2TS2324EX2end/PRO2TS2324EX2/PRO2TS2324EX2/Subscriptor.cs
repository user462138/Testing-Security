using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO2TS2324EX2;

public class Subscriptor
{
    private readonly ResultsService resultService;
    private readonly MailService mailService;
    private readonly RegisterService registerService;
    private readonly Logger logger;
    public Subscriptor(ResultsService resultService, MailService mailService, RegisterService registerService, Logger logger)
    {
        this.resultService = resultService;
        this.mailService = mailService;
        this.registerService = registerService;
        this.logger = logger;
    }
    public void Evaluate()
    {
/*        ResultsService resultService = new ResultsService();
        MailService mailService = new MailService();
        RegisterService registerService = new RegisterService();
        Logger logger = new Logger();*/
        

        string student = "janvandenpoel";
        try
        {
            Results results = resultService.GetResults(student);

            if (results.logisch_denken < 5 && results.werkethiek < 5)
            {
                mailService.SendMail(student, "Je kan beter een andere opleiding kiezen");
            }
            if (results.logisch_denken < 5)
            {
                registerService.Register(student, "Logisch Denken");
            }
            if (results.werkethiek > 5)
            {
                registerService.Register(student, "Werkethiek");
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            logger.Log(ex.Message);
        }
    }
}
