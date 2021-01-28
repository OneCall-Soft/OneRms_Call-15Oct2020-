<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

  <style>
* {box-sizing: border-box;}
body {font-family: Verdana, sans-serif;}
.mySlides {display: none;}
img {vertical-align: middle;}

/* Slideshow container */
.slideshow-container {
  max-width: 1000px;
  position: relative;
  margin: auto;
}

/* Caption text */
.text {
  color: #f2f2f2;
  font-size: 15px;
  padding: 8px 12px;
  position: absolute;
  bottom: 8px;
  width: 100%;
  text-align: center;
}

/* Number text (1/3 etc) */
.numbertext {
  color: #f2f2f2;
  font-size: 12px;
  padding: 8px 12px;
  position: absolute;
  top: 0;
}

/* The dots/bullets/indicators */
.dot {
  height: 15px;
  width: 15px;
  margin: 0 2px;
  background-color: #bbb;
  border-radius: 50%;
  display: inline-block;
  transition: background-color 0.6s ease;
}

.active {
  background-color: #717171;
}

/* Fading animation */
.fade {
  -webkit-animation-name: fade;
  -webkit-animation-duration: 1.5s;
  animation-name: fade;
  animation-duration: 1.5s;
}

@-webkit-keyframes fade {
  from {opacity: .4} 
  to {opacity: 1}
}

@keyframes fade {
  from {opacity: .4} 
  to {opacity: 1}
}

/* On smaller screens, decrease text size */
@media only screen and (max-width: 300px)
{
  .text {font-size: 11px}
}
</style>


    <div class="tab-block mb25">
            <ul class="nav nav-tabs tabs-bg">
                <li class="active">
                    <a href="#tab10_1" data-toggle="tab" aria-expanded="true">About Organization </a>
                </li>
                <li class="">
                    <a href="#tab10_2" data-toggle="tab" aria-expanded="false"><i class="fa fa-cog text-purple"></i>Contact Us</a>                        
                </li>
            </ul>

            <div class="tab-content">
                <div id="tab10_1" class="tab-pane active">
                    <p class="text-danger-darker" style="background-color:floralwhite">Founded in 1986 <b>Lipi Data Systems</b> today’s stands firms on its co-strengths of a focused company driven by customer satisfaction. A complete printing and automation solutions company Lipi has advanced its technological products to the complete range of products and services covering almost every critical printing and automation applications.</p>
                    <div class="col-md-6 text-danger-darker">
                       <h5> Vision:</h5>
    <p>To be among the Top 3 IT peripherals and automation solution company in India</p>

    <h5>Mission:</h5>
    <p>To be recognized as a quality supplier of IT peripherals & automation solution products through continuous improvement of systems & processes, development of core competence & expertise at global level.
                        </p>
    <h5>Company Overview:</h5>
    <p>Incorporated in 1986, Lipi is among the top 3 Indian IT peripheral companies of the country. For over 33 years, Lipi has been committed towards providing the best of technologies and services through indigenously designed products. Lipi has been the market leader in the Line Matrix & High-Speed Dot Matrix Printer categories for decades.
                        </p>
    <p>Lipi is recognised as the most reliable organization when it comes to the manufacturing of mission critical Products. In Lipi, quality products and services go hand-in-hand with economic performance.
                        </p>
    <p>Having presence in more than 200+ locations countrywide for direct support provides a unique strength. ISO 9001:2015 & ISO 14001:2015 are testimony to the quality that we produce.
                        </p>
    <p>Lipi made a pioneering move in the Banking Industry by setting up state of the art Manufacturing facility & Design centre for ATM, Kiosk & Self Service Terminals.
                        </p>
    <h5>E-Waste Management
                        </h5>
    <p>LIPI DATA SYSTEMS LTD. has been working in the area of safe disposal of E-waste. Since electronics items contain toxic and Hazardous Waste and Compressed Air Condensate. Safe disposal of E-waste reduces the environment pollution. The correct disposal of old product will help prevent potential negative consequences for the environment and human health. So, when an Electrical & Electronics item reaches its end-of-life, we take it back, reuse it or dismantle/recycle it. Company policy of LIPI DATA SYSTEMS LTD. supports recycling and remains committed to comply with India’s E-waste (Management) Rules 2016 as per the guideline issued by Central Pollution Control Board.
                        </p>


                    </div>

                    <div class="col-md-6">
                        <div class="slideshow-container">
                            <div class="mySlides fade">
                              <div class="numbertext"></div>
                              <img src="assets/img/kiosk/img_abt_1.jpg" alt="LIPI" style="width: 80%;"/>
                              <div class="text"></div>
                            </div>

                            <div class="mySlides fade">
                              <div class="numbertext"></div>
                              <img src="assets/img/kiosk/img_abt_2.jpg" alt="LIPI" style="width: 80%;"/>
                              <div class="text"></div>
                            </div>

                            <div class="mySlides fade">
                              <div class="numbertext"></div>
                             <img src="assets/img/kiosk/img_abt_3.jpg" alt="LIPI" style="width: 80%;"/>
                              <div class="text"></div>
                            </div>

                            <div class="mySlides fade">
                              <div class="numbertext"></div>
                             <img src="assets/img/kiosk/img_abt_4.jpg" alt="LIPI" style="width: 80%;"/>
                              <div class="text"></div>
                            </div>
                        </div>

                        <br/>
                        <div style="text-align:center">
                          <span class="dot"></span> 
                          <span class="dot"></span> 
                          <span class="dot"></span>                                     
                          <span class="dot"></span>
                        </div>

                    </div>

                </div>
                <div id="tab10_2" class="tab-pane">

                    <div class="col-md-6">
                        Mumbai,
                        <br />
                        Lipi Data Systems Ltd.
                        <br />
                        1, Mittal Chambers, Nariman Point
                        <br />
                        Mumbai - 400021, Maharashtra, India
                        <br />
                        Contact No: +91 22 22882960,22882975
                        <br />
                        Email: mumbai@lipidata.in 


                

                    </div>

                    <div class="col-md-10">                    
                        <img src="assets/img/kiosk/LipiLogo.png" alt="LIPI" style="width: 10%;">
                    </div>


                </div>

            </div>
        </div>

    <link href="assets/skin/default_skin/less/theme.css" rel="stylesheet" />

    <script>
var slideIndex = 0;
showSlides();

function showSlides()
{
  var i;
  var slides = document.getElementsByClassName("mySlides");
  var dots = document.getElementsByClassName("dot");  
  for (i = 0; i < slides.length; i++)
  {
    slides[i].style.display = "none";  
  }
  slideIndex++;
  if (slideIndex >= slides.length)
  {
      slideIndex = 1
  }    
  for (i = 0; i < dots.length; i++)
  {
    dots[i].className = dots[i].className.replace(" active", "");
  }
  slides[slideIndex-1].style.display = "block";  
  dots[slideIndex-1].className += " active";
  setTimeout(showSlides, 1500); // Change image every 2 seconds
}
</script>
</asp:Content>

