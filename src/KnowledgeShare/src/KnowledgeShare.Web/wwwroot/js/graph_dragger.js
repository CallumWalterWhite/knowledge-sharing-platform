﻿function drawGraph(data) {
    document.getElementById("graph").innerHTML = "";
    var svg = d3.select("#graph");
    var width = svg.attr("width");
    var height = svg.attr("height");

    svg = svg.call(d3.zoom().on("zoom", zoomed)).append("g");

    svg.append("defs").append("marker")
        .attr("id", "arrow")
        .attr("viewBox", "0 -5 10 10")
        .attr("refX", 20)
        .attr("refY", 0)
        .attr("markerWidth", 8)
        .attr("markerHeight", 8)
        .attr("orient", "auto")
        .append("svg:path")
        .attr("d", "M0,-5L10,0L0,5");

    var color = d3.scaleOrdinal(d3.schemeCategory10);

    var simulation = d3.forceSimulation()
        .force("link", d3.forceLink().id(function(d) { return d.id; }))
        .force("charge", d3.forceManyBody().strength(-50))
        .force("center", d3.forceCenter(width / 2, height / 2));
    function createGraph (error, graph) {
        if (error) throw error;

        var link = svg.append("g")
            .attr("class", "links")
            .selectAll("line")
            .data(graph.links)
            .enter().append("line")
            .attr("stroke", function(d) { return color(d.type); })
            .attr("marker-end", "url(#arrow)");
        var node = svg.append("g")
            .attr("class", "nodes")
            .selectAll("circle")
            .data(graph.nodes)
            .enter().append("circle")
            .attr("r", 10)
            .attr("fill", function(d) { if (d.root == "true") return color(d.root); return color(d.type); })
            .call(d3.drag()
                .on("start", dragstarted)
                .on("drag", dragged)
                .on("end", dragended));

        var text = svg.append("g").attr("class", "labels").selectAll("g")
            .data(graph.nodes)
            .enter().append("g");

        text.append("text")
            .attr("x", 14)
            .attr("y", ".31em")
            .style("font-family", "sans-serif")
            .style("font-size", "0.7em")
            .text(function(d) { return d.id; });

        node.on("click",function(d){
            if (d.type == 2) {
                load_post("/articlepost/" + d.postIdentifer + "");
            }
            else if (d.type == 3) {
                load_post("/bookpost/" + d.postIdentifer + "");
            }
            else if (d.type == 4) {
                load_post("/freeformpost/" + d.postIdentifer + "");
            }
            console.log(d);
            console.log("clicked", d.id);
        });
            
        var firstsearch = true;
        function search(){
            var txtName = d3.select("#txtName").node().value;
            if (txtName.length > 0) {
                node.style("fill", function(d) {
                    if (!isStringLike(txtName, d.id)) {
                        return "white";
                    }
                })
            }
        }
        d3.select("#tryit").on("click", function() {
            if (!firstsearch){
                drawGraph(data);
                setTimeout(search, 1000);
            }
            else{
                search();
                firstsearch = false;
            }
        })
        
        function load_post(url){
            if (!window["loading_page"]){
                window["loading_page"] = true;
                window.location = url;   
            }
        }


        node.append("title")
            .text(function(d) { return d.id; });
        
        simulation
            .nodes(graph.nodes)
            .on("tick", ticked);

        simulation.force("link")
            .links(graph.links);


        function ticked() {
            link
                .attr("x1", function(d) { return d.source.x; })
                .attr("y1", function(d) { return d.source.y; })
                .attr("x2", function(d) { return d.target.x; })
                .attr("y2", function(d) { return d.target.y; });

            node
                .attr("cx", function(d) { return d.x; })
                .attr("cy", function(d) { return d.y; });

            text
                .attr("transform", function(d) { return "translate(" + d.x + "," + d.y + ")"; })


        }
    }
    
    function dragstarted(d) {
        if (!d3.event.active) simulation.alphaTarget(0.3).restart();
        d.fx = d.x;
        d.fy = d.y;
    }

    function dragged(d) {
        d.fx = d3.event.x;
        d.fy = d3.event.y;
    }

    function dragended(d) {
        if (!d3.event.active) simulation.alphaTarget(0);
        d.fx = null;
        d.fy = null;
    }
    function zoomed() {
        svg.attr("transform", "translate(" + d3.event.transform.x + "," + d3.event.transform.y + ")" + " scale(" + d3.event.transform.k + ")");
    }

    createGraph(false, JSON.parse(data));
}

function isStringLike(str, pattern) {
    // Escape special characters in the pattern
    const escapedPattern = pattern.toLowerCase().replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    return escapedPattern.includes(str.toLowerCase());
}