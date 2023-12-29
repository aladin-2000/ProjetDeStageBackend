using backEnd.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

public class CveController:ControllerBase{


        private static readonly HttpClient httpClient=new HttpClient();

        private readonly CpeController cpeController;


        public CveController(CpeController cpeController){
            this.cpeController=cpeController;

        }



}