

class App {

    previousResponseText = null;

    /**
     * @type {Notification}
     */
    notification = null;
    
    run() {
        
        this.notification = new Notification();

        const loopTime = 1000;

        setInterval(()=> this.onLoop(), loopTime);
        
        console.log('Waiting for updates');


    }

    async onLoop() {

        const response = await fetch('http://localhost:5000/api/lastshift');

        if (response.ok) {

            const text = await response.text();
            
            if (this.previousResponseText !== null && this.previousResponseText !== text) {
                
                console.log('State has changed!');

                const object = JSON.parse(text);

                const data = new ShiftData(object);
                
                console.log('New state:', data);

                const message = `${data.employeeName} ${data.isJoin ? 'entrou' : 'saiu'}`;
                
                this.notification.show(message);

            }
            
            this.previousResponseText = text;

        }
        else {
            console.log('Response was not ok');
        }

    }

}

class Notification {

    __notificationElement = document.querySelector('#notification-div');
    __timeout = null;
    
    show(message) {
        
        console.log('Showing notification:', message)
        
        this.__notificationElement.innerText = message;
        this.__notificationElement.style.opacity = '1';
        
        clearTimeout(this.__timeout);
        
        this.__timeout = setTimeout(() => this.__hide(), 3000);
        
    }
    
    __hide() {
        console.log('Hiding previous notification');
        this.__notificationElement.style.opacity = '0';
    }
}


class ShiftData {
    
    constructor(source) {
        
        this.employeeName = source['employee_name'];
        this.isJoin = source['is_join'];
    }
    
}

const app = new App();


$(document).ready(() => {
    console.log('Document ready!');

    app.run();
})
