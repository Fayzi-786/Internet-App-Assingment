
let eventsData = [];
let randomEvent;

function fetchJSONData() {
  return fetch("./event_data.json")
    .then((res) => {
      if (!res.ok) {
        throw new Error(`HTTP error! Status: ${res.status}`);
      }
      return res.json();
    })
    .then((data) => {
      console.log("Fetched data:", data);
      eventsData = data;
    })
    .catch((error) => console.error("Unable to fetch data:", error));
}
function createEventCard(event) {
  return `
   <div class="event-card">
   <div class="image-wrapper">
   <img src="./Assets/images/${event.image}" alt="Event Image" class="event-image" />
   </div>
    <div class="event-details">
    
    <h3 class="event-title">${event.title}</h3>
    <p class="event-date">${event.date}</p>
    <button class="event-button" onclick="handleLearnMore(${event.id})">Learn More</button>
  </div>
</div>
  `;
}
function handleLearnMore(id) {
  window.location.href = `./Event_Details/Event-details.html?id=${id}`
}
function renderEvents() {
  oldEvent = randomEvent;
  randomEvent = eventsData.events[Math.floor(Math.random() * eventsData.events.length)];
  if (oldEvent == randomEvent) {
    renderEvents()
  }
  const container = document.getElementById("event-container");
  const html = createEventCard(randomEvent)
  container.innerHTML = html

}

 const customizationPanel =  document.querySelector('.customization-panel');
    const overlay = document.querySelector('.overlay');
document.addEventListener("DOMContentLoaded", async () => {
  const themeToggle = document.createElement('button');
  themeToggle.id = 'theme-toggle';
  themeToggle.innerHTML = `
    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
      <circle cx="12" cy="12" r="5"/>
      <path d="M12 1v2M12 21v2M4.2 4.2l1.4 1.4M18.4 18.4l1.4 1.4M1 12h2M21 12h2M4.2 19.8l1.4-1.4M18.4 5.6l1.4-1.4"/>
    </svg>
  `;
  document.body.appendChild(themeToggle);
  const settings = document.createElement('button');
  settings.id = 'settings';
  settings.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
  <path
    d="M10 12.5a2.5 2.5 0 1 1 0-5 2.5 2.5 0 0 1 0 5zm0-1a1.5 1.5 0 1 0 0-3 1.5 1.5 0 0 0 0 3z"
    fill="currentColor"
  />
  <path
    d="M8.5 1.5A.5.5 0 0 1 9 1h2a.5.5 0 0 1 .5.5v1.062a6.44 6.44 0 0 1 1.64.678l.752-.752a.5.5 0 0 1 .707 0l1.414 1.414a.5.5 0 0 1 0 .707l-.752.752a6.44 6.44 0 0 1 .678 1.64H17a.5.5 0 0 1 .5.5v2a.5.5 0 0 1-.5.5h-1.062a6.44 6.44 0 0 1-.678 1.64l.752.752a.5.5 0 0 1 0 .707l-1.414 1.414a.5.5 0 0 1-.707 0l-.752-.752a6.44 6.44 0 0 1-1.64.678V17a.5.5 0 0 1-.5.5H9a.5.5 0 0 1-.5-.5v-1.062a6.44 6.44 0 0 1-1.64-.678l-.752.752a.5.5 0 0 1-.707 0L3.987 14.598a.5.5 0 0 1 0-.707l.752-.752a6.44 6.44 0 0 1-.678-1.64H3a.5.5 0 0 1-.5-.5v-2A.5.5 0 0 1 3 8.5h1.062a6.44 6.44 0 0 1 .678-1.64l-.752-.752a.5.5 0 0 1 0-.707L5.402 3.987a.5.5 0 0 1 .707 0l.752.752a6.44 6.44 0 0 1 1.64-.678V3a.5.5 0 0 1 .5-.5zm1.5.5v1.25a.5.5 0 0 1-.375.484 5.45 5.45 0 0 0-2.056.85.5.5 0 0 1-.606-.044l-.883-.883L4.667 4.67l.883.883a.5.5 0 0 1 .044.606 5.45 5.45 0 0 0-.85 2.056.5.5 0 0 1-.484.375H3v2h1.25a.5.5 0 0 1 .484.375 5.45 5.45 0 0 0 .85 2.056.5.5 0 0 1-.044.606l-.883.883 1.414 1.414.883-.883a.5.5 0 0 1 .606-.044 5.45 5.45 0 0 0 2.056.85.5.5 0 0 1 .375.484V17h2v-1.25a.5.5 0 0 1 .375-.484 5.45 5.45 0 0 0 2.056-.85.5.5 0 0 1 .606.044l.883.883 1.414-1.414-.883-.883a.5.5 0 0 1-.044-.606 5.45 5.45 0 0 0 .85-2.056.5.5 0 0 1 .484-.375H17v-2h-1.25a.5.5 0 0 1-.484-.375 5.45 5.45 0 0 0-.85-2.056.5.5 0 0 1 .044-.606l.883-.883-1.414-1.414-.883.883a.5.5 0 0 1-.606.044 5.45 5.45 0 0 0-2.056-.85.5.5 0 0 1-.375-.484V2h-2z"
    fill="currentColor"
  />
</svg>`;
  document.body.appendChild(settings);
  settings.addEventListener('click', () => {
   
    customizationPanel.classList.toggle('active');
    overlay.classList.toggle('active');
  })
  await fetchJSONData();
  renderEvents();
  const savedTheme = localStorage.getItem('Mode');
  if (savedTheme === "dark") { document.documentElement.classList.add('dark-mode') }
  if (savedTheme === "light") { document.documentElement.classList.remove('dark-mode') }

  document.querySelector("#theme-toggle").addEventListener("click", () => {
    document.documentElement.classList.toggle("dark-mode")
    const isDarkMode = document.documentElement.classList.contains("dark-mode")
    if (isDarkMode) {
      localStorage.setItem("Mode", "dark")

    } else {
      localStorage.setItem("Mode", "light")
    }

  });
 const savedFontSize = localStorage.getItem("headerFontSize");
  const savedBgColor = localStorage.getItem("headerBgColor");

  if (savedFontSize) {
    document.querySelectorAll('.nav-link').forEach(link => {
      link.style.fontSize = `${savedFontSize}px`;
    });
    fontSizeInput.value = savedFontSize;
  }

  if (savedBgColor) {
    header.style.backgroundColor = savedBgColor;
    bgColorInput.value = savedBgColor;
  }
});
const header = document.querySelector(".header");

const fontSizeInput = document.getElementById("fontSize");
const bgColorInput = document.getElementById("bgColor");
const saveButton = document.getElementById("savePreferences");
saveButton.addEventListener("click", () => {
  const fontSize = fontSizeInput.value;
  const bgColor = bgColorInput.value;

  if (fontSize) {
    document.querySelectorAll('.nav-link').forEach(link => {
      link.style.fontSize = `${fontSize}px`;
    });
    localStorage.setItem("headerFontSize", fontSize);
  }

  if (bgColor) {
    header.style.backgroundColor = bgColor;
    localStorage.setItem("headerBgColor", bgColor);
  }
    customizationPanel.classList.remove('active');
  overlay.classList.remove('active');
  
});
setInterval(renderEvents, 5000)




document.querySelector("#close_modal").addEventListener('click', () => {
  customizationPanel.classList.remove('active');
  overlay.classList.remove('active');
})
