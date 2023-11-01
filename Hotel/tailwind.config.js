const defaultTheme = require('tailwindcss/defaultTheme');

module.exports = {
    content: [
       './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        "./node_modules/flowbite/**/*.js"
],
    theme: {
        colors: {
            hhkred: '#86161a',
            hhkbeige: '#ede3d5',
            hhkorange: '#edc46b',
            transparent: 'transparent',
            current: 'currentColor',
            'white': '#ffffff',
            'purple': '#3f3cbb',
            'midnight': '#121063',
            'metal': '#565584',
            'tahiti': '#3ab7bf',
            'silver': '#ecebff',
            'bubble-gum': '#ff77e9',
            'bermuda': '#78dcca',
        },
        extend: {
            fontFamily: {
                libre: ['"Libre Baskerville"', ...defaultTheme.fontFamily.sans],
                caladea: ['Caladea', ...defaultTheme.fontFamily.sans]

            },
            colors: {
                'royal-orange': '#D0D4D0',
            }
        },

    },
    daisyui: {
        themes: [
            {
                mytheme: {

                    "primary": "#fb923c",

                    "secondary": "#991b1b",

                    "accent": "#eab308",

                    "neutral": "#2b3440",

                    "base-100": "#ffffff",

                    "info": "#3abff8",

                    "success": "#65a30d",

                    "warning": "#ea580c",

                    "error": "#e11d48",
                },
            },
        ],
    },
    plugins: [
        require('flowbite/plugin')
    ],
}