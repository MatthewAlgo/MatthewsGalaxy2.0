import { Component } from '@angular/core';

@Component({
  selector: 'app-aboutmecomponent',
  templateUrl: './aboutmecomponent.component.html',
  styleUrls: ['./aboutmecomponent.component.css']
})
export class AboutmecomponentComponent {
  markdownContent: string = `
    # About Me 😊

    Hi there! I'm Matei-Alexandru Dinu, a software developer based in Bucharest, Romania. Currently, I'm pursuing a BSc in Computer Science at the University of Bucharest, expected to graduate in 2025. I have a keen interest in IoT connectivity and dynamic web applications, with professional experience at NXP Semiconductors.

    ## Education 📚
    - **BSc Computer Science**, University of Bucharest, Romania (Oct 2022 - Jun 2025)
      - Third year, honors student

    ## Professional Experience 💼
    ### Software Engineer Intern at NXP Semiconductors (March 2023 - Present)
    - Developed a full-stack IoT application using Python and Flutter.
    - Designed a dynamic packet routing system leveraging multiprocessing and shared memory.
    - Enabled OTA updates with multiple client connections via WebSockets.
    - Coordinated code reviews and backend improvements, developed a frontend in Angular.
    - Represented the Matter team at the Innovation World Tour 2024.

    ## Skills 🚀
    ### Programming Languages and Tools
    - **Languages:** C/C++, Python, Java, TypeScript
    - **Frameworks:** Angular, Flutter, .NET Core, .NET MVC
    - **Technologies:** Docker, SQL, Linux, IoT
    - **Libraries:** SFML, ImGUI, FTxUI
    - **Development:** Backend Development, Multithreading, VIM-like editors

    ### Languages
    - Romanian (Native)
    - English (Advanced, Level C1)
    - French (Classroom study)

    ## Projects 🌟
    - **OpenTube**: A YouTube client in Flutter for Android supporting downloads, streaming, and lazy-loaded search results.
    - **AlienInvaders**: A C++ 2D shooter game using SFML, featuring multithreading and mutexes, and awarded in programming contests.
    - **AtAGlance**: Java-based mobile app for weather and news using APIs, with data retrieved on-demand.
    - **PiCr**: A C++ terminal text editor with VIM-like features, developed with FTxUI for functionality and multithreading.
    - **SoftDoc**: Full-stack .NET web app for project management, featuring CRUD operations and role-based user management.

    ## Hobbies 🎮
    - Reading 📚
    - Coding 💻
    - Working out 🏋️
    - Investing 📈

    ## Awards 🏆
    - **3rd Place** at Prosoft national programming contest for 'AlienInvaders' (Mar 2022)
    - **2nd Place** at InfoEducatie programming contest - Bucharest phase (May 2022)
    - **FIICode Participation** with highest creativity and code design score (May 2022)
    - Qualified 3 consecutive years at the Center of Excellence in Informatics (2019-2021)

    ## Contact 📞
    - Email: [matthewdy0101@gmail.com](mailto:matthewdy0101@gmail.com)
    - LinkedIn: [linkedin.com/in/dinu-matei-alexandru](https://www.linkedin.com/in/dinu-matei-alexandru)
    - Phone: +40 775131805

    Feel free to reach out to me for collaboration or just to say hi! 😊
  `;
}
