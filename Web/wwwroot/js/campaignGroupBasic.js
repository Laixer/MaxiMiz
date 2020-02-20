// Switch between forms without leaving the page

const form1 = document.querySelector('#formAccount');
const form2 = document.querySelector('#formMarketing');
const form3 = document.querySelector('#formOverview');
const stepMarketing = document.querySelector('.step-marketing');
const iconMarketing = stepMarketing.querySelector('.marketing-icon');
const pMarketing = stepMarketing.querySelector('p');
const hr = document.querySelector('.hr');


form1.querySelector('button').addEventListener('click', function (e) {
  e.preventDefault()

  form1.style.left = "-150px";
  form1.style.opacity = "0";

  setTimeout(function () {
    form2.style.display = "grid";
  }, 400);

  setTimeout(function () {
    form1.style.display = "none";
    form2.style.opacity = "1";
    form2.style.left = "0";
  }, 500);

  iconMarketing.style.background = "var(--primary-color)";
  pMarketing.style.color = "var(--primary-color)"
  iconMarketing.querySelector('img').src = "../../images/marketing-icon-light.svg";
  hr.style.background = "var(--primary-color)";

  console.log(iconMarketing);
});

form2.querySelector('button').addEventListener('click', function (e) {
  e.preventDefault();
  form2.style.left = "-150px";
  form2.style.opacity = "0";

  setTimeout(function () {
    form3.style.display = "grid";
  }, 400);

  setTimeout(function () {
    form2.style.display = "none";
    form3.style.opacity = "1";
    form3.style.left = "0";
  }, 500);
});