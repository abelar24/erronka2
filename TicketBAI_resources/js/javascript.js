if (localStorage.getItem("xmls") == null) {
    const xmls = [];
    localStorage.setItem("xmls", JSON.stringify(xmls));
} else {
    const xmls = JSON.parse(localStorage.getItem("xmls"));
}

function getPropertyValue(property) {
    return property ? property.text : '';
}

function sortuTaula() {
    const tiketak = JSON.parse(localStorage.getItem("xmls"));
    const tbody = document.querySelector('#facturas-table tbody');

    tiketak.forEach((ticket, index) => {
        const ticketa = ticket.Ticketa || {};

        const row = document.createElement('tr');

        row.innerHTML = `
            <td>${ticketa.Baskula?.text || ''}</td>
            <td>${ticketa.Produktua?.text || ''}</td>
            <td>${ticketa.Saltzailea?.text || ''}</td>
            <td>${ticketa.PrezioKiloko?.text || ''}</td>
            <td>${ticketa.Pisua?.text || ''}</td>
            <td>${ticketa.Totala?.text || ''}</td>
            <td>${ticketa.TicketZenbakia?.text || ''}</td>
            <td>${ticketa.Eguna?.text || ''}</td>
            <td><button class="btn btn-primary" onclick="joanFakturara(${index})">Ikusi</button></td>
        `;

        tbody.appendChild(row);
    });
}

function parseXMLtoJS(xmlString) {
    try {
        const parser = new DOMParser();
        const xmlDoc = parser.parseFromString(xmlString, "application/xml");

        const parserError = xmlDoc.getElementsByTagName("parsererror");
        if (parserError.length > 0) {
            throw new Error("Invalid XML format: " + parserError[0].textContent);
        }

        function traverse(node) {
            const obj = {};

            if (node.attributes && node.attributes.length > 0) {
                Array.from(node.attributes).forEach(attr => {
                    obj[`@${attr.name}`] = attr.value;
                });
            }

            if (node.children.length > 0) {
                Array.from(node.children).forEach(child => {
                    const childName = child.nodeName;
                    const childObj = traverse(child);

                    if (obj[childName]) {
                        if (!Array.isArray(obj[childName])) {
                            obj[childName] = [obj[childName]];
                        }
                        obj[childName].push(childObj);
                    } else {
                        obj[childName] = childObj;
                    }
                });
            } else {
                obj["text"] = node.textContent.trim();
            }

            return obj;
        }

        return traverse(xmlDoc.documentElement);
    } catch (error) {
        console.error("Error parsing XML:", error.message);
        return null;
    }
}

async function exekutatu() {
    let xmls = [];
    try {
        const xmls1 = JSON.parse(localStorage.getItem("xmls"));

        const response = await fetch('../xmls/facturas.xml');

        if (!response.ok) {
            throw new Error(`Failed to fetch XML: ${response.statusText}`);
        }

        const xmlText = await response.text();
        const parsedObject = parseXMLtoJS(xmlText);

        localStorage.setItem("tiketak", JSON.stringify(parsedObject));

        const tiketak = parsedObject.InvoiceFile;
        for (let i = 0; i < tiketak.length; i++) {
            const response = await fetch('../xmls/' + tiketak[i].text);

            if (!response.ok) {
                throw new Error(`Failed to fetch XML: ${response.statusText}`);
            }

            const xmlText = await response.text();
            const parsedTicket = parseXMLtoJS(xmlText);
            if (xmls1.length == 0) {
                xmls.push(parsedTicket);
            }
        }
        if (xmls.length != 0) {
            localStorage.setItem("xmls", JSON.stringify(xmls));
        }
    } catch (error) {
        console.error("Error fetching XML:", error);
        return null;
    }

    sortuTaula();
}

function joanFakturara(id) {
    window.location.href = `faktura.html?ticketId=${id}`;
}

function sortuTiketa() {
    const urlParams = new URLSearchParams(window.location.search);
    const ticketId = urlParams.get('ticketId');

    const tiketak = JSON.parse(localStorage.getItem("xmls"));

    if (tiketak && tiketak.length > ticketId) {
        const ticket = tiketak[ticketId];
        const ticketa = ticket.Ticketa || {};

        document.getElementById("ticket-details").innerHTML = `
            <h4>Tiketaren informazioa</h4>
            <p><strong>Baskula:</strong> ${ticketa.Baskula?.text || ''}</p>
            <p><strong>Produktua:</strong> ${ticketa.Produktua?.text || ''}</p>
            <p><strong>Saltzailea:</strong> ${ticketa.Saltzailea?.text || ''}</p>
            <p><strong>Prezioa Kiloko:</strong> ${ticketa.PrezioKiloko?.text || ''}</p>
            <p><strong>Pisua:</strong> ${ticketa.Pisua?.text || ''}</p>
            <p><strong>Totala:</strong> ${ticketa.Totala?.text || ''}</p>
            <p><strong>Tiket Zenbakia:</strong> ${ticketa.TicketZenbakia?.text || ''}</p>
            <p><strong>Eguna:</strong> ${ticketa.Eguna?.text || ''}</p>
        `;

        const productsTable = document.getElementById("products");
        productsTable.innerHTML = '';
        if (ticketa.Productuak && ticketa.Productuak.Produktua) {
            ticketa.Productuak.Produktua.forEach(product => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${product.Quantity?.text || ''}</td>
                    <td>${product.Name?.text || ''}</td>
                    <td class="text-end">${product.UnitPrice?.text || ''}</td>
                    <td class="text-end">${product.Amount?.text || ''}</td>
                `;
                productsTable.appendChild(row);
            });
        }
    } else {
        alert("No hay tickets disponibles en LocalStorage.");
    }
}

function atzeraBueltatu() {
    window.location.href = "index.html";
}